using AutoMapper;
using BloodBankApp.Areas.Identity.Services;
using BloodBankApp.Areas.SuperAdmin.Services;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Data;
using BloodBankApp.Mapping;
using BloodBankApp.Models;
using BloodBankApp.Services;
using BloodBankApp.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using BloodBankApp.Areas.Identity.Services.Interfaces;
using BloodBankApp.Areas.Services;
using BloodBankApp.Areas.Services.Interfaces;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using BloodBankApp.Areas.HospitalAdmin.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using BloodBankApp.Areas.HospitalAdmin.Services;
using BloodBankApp.Areas.SuperAdmin.Permission;
using BloodBankApp.Hubs;

using System.Text.Json.Serialization;
using BloodBankApp.Areas.HospitalAdmin.Services.Interfaces;
using BloodBankApp.Areas.HospitalAdmin.Services;

namespace BloodBankApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<ApplicationDbContext>
            (options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole<Guid>>()
                .AddRoles<IdentityRole<Guid>>()
                .AddRoleManager<RoleManager<IdentityRole<Guid>>>()
                .AddSignInManager<SignInManager<User>>()
                .AddUserManager<UserManager<User>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            services.AddScoped<IDonatorService, DonatorService>();
            services.AddScoped<IHospitalService, HospitalService>();
            services.AddScoped<IBloodTypesService, BloodTypesService>();
            services.AddScoped<ICitiesService, CitiesService>();
            services.AddScoped<IDonorsService, DonorsService>();
            services.AddScoped<IRolesService, RolesService>();
            services.AddScoped<IMedicalStaffService, MedicalStaffService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IStatisticsService, StatisticsService>();
            services.AddScoped<ISignInService, SignInService>();
            services.AddScoped<ISuggestionsService, SuggestionsService>();
            services.AddScoped<IAvailabilityService, AvailabilityService>();
            services.AddScoped<IHospitalAdminService, HospitalAdminService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IQuestionService, QuestionsService>();

            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

            services.AddRazorPages();
            services.AddNotyf(config => {
                config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.TopRight; 
            });

            var mapperConfig = new MapperConfiguration(mapper =>
            {
                mapper.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseNotyf();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();

                endpoints.MapControllerRoute(
                    name: "SuperAdmin",
                    pattern: "{area:exists}/{controller=AdminHome}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                 name: "Donator",
                 pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapHub<ChatHub>("/chatHub");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
