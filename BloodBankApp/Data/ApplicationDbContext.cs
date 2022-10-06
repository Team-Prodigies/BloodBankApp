using BloodBankApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace BloodBankApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

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
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Code> Codes { get; set; }
        public DbSet<DonationRequests> DonationRequest { get; set; }
        public DbSet<Issue> Issues { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<BloodDonation>(bd =>
            {
                bd.HasOne(b => b.Donor)
               .WithMany(d => d.BloodDonations)
               .HasForeignKey(fk => fk.DonorId)
               .HasConstraintName("DonorDonations")
               .OnDelete(DeleteBehavior.NoAction);

                bd.HasOne(b => b.DonationPost)
               .WithMany(d => d.BloodDonations)
               .HasForeignKey(fk => fk.DonationPostId)
               .HasConstraintName("PostDonations")
               .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<BloodReserve>(br =>
            {
                br.HasOne(b => b.Hospital)
               .WithMany(d => d.BloodReserves)
               .HasForeignKey(fk => fk.HospitalId)
               .HasConstraintName("HospitalReserves")
               .OnDelete(DeleteBehavior.Cascade);

                br.HasOne(b => b.BloodType)
               .WithMany(d => d.BloodReserves)
               .HasForeignKey(fk => fk.BloodTypeId)
               .HasConstraintName("TypeReserves")
               .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<DonationPost>(dp =>
            {
                dp.HasOne(b => b.Hospital)
               .WithMany(d => d.DonationPosts)
               .HasForeignKey(fk => fk.HospitalId)
               .HasConstraintName("HospitalPosts")
               .OnDelete(DeleteBehavior.Cascade);

                dp.HasOne(b => b.BloodType)
               .WithMany(d => d.DonationPosts)
               .HasForeignKey(fk => fk.BloodTypeId)
               .HasConstraintName("TypePosts")
               .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Donor>(d =>
            {
                d.HasOne(b => b.BloodType)
               .WithMany(d => d.Donors)
               .HasForeignKey(fk => fk.BloodTypeId)
               .HasConstraintName("TypesDonors")
               .OnDelete(DeleteBehavior.Cascade);

                d.HasOne(d => d.User)
               .WithOne(p => p.Donor)
               .HasForeignKey<Donor>(d => d.DonorId)
               .HasConstraintName("UserDonor")
               .OnDelete(DeleteBehavior.Cascade);

                d.HasOne(b => b.BloodType)
               .WithMany(d => d.Donors)
               .HasForeignKey(fk => fk.BloodTypeId)
               .HasConstraintName("DonorsBloodTypes")
               .OnDelete(DeleteBehavior.Cascade);

                d.HasIndex(u => u.PersonalNumber).IsUnique();
            });

            builder.Entity<MedicalStaff>(ms =>
            {
                ms.HasOne(d => d.User)
               .WithOne(p => p.MedicalStaff)
               .HasForeignKey<MedicalStaff>(d => d.MedicalStaffId)
               .HasConstraintName("UserMedicalStaff")
               .OnDelete(DeleteBehavior.Cascade);

                ms.HasOne(b => b.Hospital)
               .WithMany(d => d.MedicalStaff)
               .HasForeignKey(fk => fk.HospitalId)
               .HasConstraintName("MedicalStaffHospital")
               .OnDelete(DeleteBehavior.Cascade);
            });

            

            builder.Entity<Hospital>(h =>
            {
                h.HasOne(b => b.Location)
               .WithMany(d => d.Hospitals)
               .HasForeignKey(fk => fk.LocationId)
               .HasConstraintName("LocationHospitals")
               .OnDelete(DeleteBehavior.Cascade);

                h.HasOne(b => b.City)
               .WithMany(d => d.Hospitals)
               .HasForeignKey(fk => fk.CityId)
               .HasConstraintName("CityHospitals")
               .OnDelete(DeleteBehavior.Cascade);

                h.HasIndex(u => u.HospitalCode).IsUnique();
            });

            builder.Entity<Notification>()
           .HasOne(b => b.DonationPost)
           .WithMany(d => d.Notifications)
           .HasForeignKey(fk => fk.DonationPostId)
           .HasConstraintName("DonationNotifications")
           .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Question>()
           .HasOne(b => b.HealthFormQuestionnaire)
           .WithMany(d => d.Questions)
           .HasForeignKey(fk => fk.HealthFormQuestionnaireId)
           .HasConstraintName("FormQuestions")
           .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Message>()
            .HasOne(bc => bc.Donor)
            .WithMany(b => b.Messages)
            .HasForeignKey(bc => bc.DonorId)
            .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Message>()
            .HasOne(bc => bc.Hospital)
            .WithMany(c => c.Messages)
            .HasForeignKey(bc => bc.HospitalId)
            .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Code>()
                .HasOne(d => d.Donor)
                .WithOne(c => c.Code)
                .HasForeignKey<Code>(c => c.CodeId)
                .HasConstraintName("CodeDonor")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Code>().HasIndex(u => u.CodeValue).IsUnique();

            builder.Entity<DonationRequests>(dr => {
                dr.HasOne(b => b.Donor)
               .WithMany(d => d.DonationRequests)
               .HasForeignKey(fk => fk.DonorId)
               .HasConstraintName("DonorDonations")
               .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}