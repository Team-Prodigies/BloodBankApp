using BloodBankApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Data {
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    { 
    
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) {

        }
        public DbSet<Donor> Donors { get; set; }

        public DbSet<BloodDonation> BloodDonations { get; set; }

        public DbSet<BloodReserve> BloodReserves { get; set; }

        public DbSet<BloodType> BloodTypes { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<DonationPost> DonationPosts { get; set; }

        public DbSet<HealthFormQuestionnaire> HealthFormQuestionnaires { get; set; }

        public DbSet<Hospital> Hospitals { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<MedicalStaff> MedicalStaffs { get; set; }

      //  public DbSet<Message> Messages { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<SuperAdmin> SuperAdmins { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<BloodDonation>()
                .HasOne(b => b.Donor)
                .WithMany(d => d.BloodDonations)
                .HasForeignKey(fk => fk.DonorId)
                .HasConstraintName("DonorDonations")
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<BloodDonation>()
                .HasOne(b => b.DonationPost)
                .WithMany(d => d.BloodDonations)
                .HasForeignKey(fk => fk.DonationPostId)
                .HasConstraintName("DonationsPosts")
                .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<BloodReserve>()
                .HasOne(b => b.Hospital)
                .WithMany(d => d.BloodReserves)
                .HasForeignKey(fk => fk.HospitalId)
                .HasConstraintName("HospitalReserves")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<BloodReserve>()
                .HasOne(b => b.BloodType)
                .WithMany(d => d.BloodReserves)
                .HasForeignKey(fk => fk.BloodTypeId)
                .HasConstraintName("TypeReserves")
                .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<DonationPost>()
               .HasOne(b => b.Hospital)
               .WithMany(d => d.DonationPosts)
               .HasForeignKey(fk => fk.HospitalId)
               .HasConstraintName("HospitalDonations")
               .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<DonationPost>()
              .HasOne(b => b.BloodType)
              .WithMany(d => d.DonationPosts)
              .HasForeignKey(fk => fk.BloodTypeId)
              .HasConstraintName("PostTypes")
              .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<Donor>()
             .HasOne(b => b.HealthFormQuestionnaire)
             .WithOne(d => d.Donor)
              .HasForeignKey<Donor>(d => d.DonorId)
             .HasConstraintName("FormDonorr")
             .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Donor>()
              .HasOne(b => b.BloodType)
              .WithMany(d => d.Donors)
              .HasForeignKey(fk => fk.BloodTypeId)
              .HasConstraintName("DonorsTypes")
              .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<Donor>()
                .HasOne(d => d.User)
               .WithOne(p => p.Donor)
               .HasForeignKey<Donor>(d => d.DonorId)
               .HasConstraintName("UserDonorr")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SuperAdmin>()
                .HasOne(d => d.User)
               .WithOne(p => p.SuperAdmin)
               .HasForeignKey<SuperAdmin>(d => d.SuperAdminId)
               .HasConstraintName("UserSuperAdminn")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MedicalStaff>()
                .HasOne(d => d.User)
               .WithOne(p => p.MedicalStaff)
               .HasForeignKey<MedicalStaff>(d => d.MedicalStaffId)
               .HasConstraintName("UserMedicalStafff")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Donor>()
            .HasOne(b => b.BloodType)
            .WithMany(d => d.Donors)
            .HasForeignKey(fk => fk.BloodTypeId)
            .HasConstraintName("DonorsTypes")
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<HealthFormQuestionnaire>()
             .HasOne(b => b.Donor)
             .WithOne(d => d.HealthFormQuestionnaire)
             .HasForeignKey<HealthFormQuestionnaire>(d => d.HealthFormQuestionnaireId)
             .HasConstraintName("DonorrForms")
             .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<Hospital>()
            .HasOne(b => b.Location)
            .WithMany(d => d.Hospitals)
            .HasForeignKey(fk => fk.LocationId)
            .HasConstraintName("DonorsLocationn")
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Hospital>()
            .HasOne(b => b.City)
            .WithMany(d => d.Hospitals)
            .HasForeignKey(fk => fk.CityId)
            .HasConstraintName("CityHospitals")
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Hospital>()
            .HasIndex(u => u.HospitalCode).IsUnique();

            builder.Entity<MedicalStaff>()
            .HasOne(b => b.Hospital)
            .WithMany(d => d.MedicalStaff)
            .HasForeignKey(fk => fk.HospitalId)
            .HasConstraintName("MedicalHospital")
            .OnDelete(DeleteBehavior.Cascade);

            /*

               builder.Entity<Message>()
                        .HasOne(b => b.Donor)
                        .WithMany(d => d.Messages)
                        .HasForeignKey(fk => fk.DonorId)
                        .HasConstraintName("DonorsMessage")
                        .OnDelete(DeleteBehavior.Cascade);

                        builder.Entity<Message>()
                        .HasOne(b => b.MedicalStaff)
                        .WithMany(d => d.Messages)
                        .HasForeignKey(fk => fk.MedicalStaffId)
                        .HasConstraintName("MessageMedical")
                        .OnDelete(DeleteBehavior.Cascade);
             */



            builder.Entity<Notification>()
            .HasOne(b => b.DonationPost)
            .WithMany(d => d.Notifications)
            .HasForeignKey(fk => fk.DonationPostId)
            .HasConstraintName("donationnotif")
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Question>()
           .HasOne(b => b.HealthFormQuestionnaire)
           .WithMany(d => d.Questions)
           .HasForeignKey(fk => fk.HealthFormQuestionnaireId)
           .HasConstraintName("formqst")
           .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
