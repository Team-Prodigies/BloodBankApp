﻿// <auto-generated />
using System;
using BloodBankApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BloodBankApp.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220914090510_Messages-table-fix")]
    partial class Messagestablefix
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BloodBankApp.Models.BloodDonation", b =>
                {
                    b.Property<Guid>("BloodDonationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<DateTime>("DonationDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("DonationPostId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DonorId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("BloodDonationId");

                    b.HasIndex("DonationPostId");

                    b.HasIndex("DonorId");

                    b.ToTable("BloodDonations");
                });

            modelBuilder.Entity("BloodBankApp.Models.BloodReserve", b =>
                {
                    b.Property<Guid>("BloodReserveId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<Guid>("BloodTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("HospitalId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("BloodReserveId");

                    b.HasIndex("BloodTypeId");

                    b.HasIndex("HospitalId");

                    b.ToTable("BloodReserves");
                });

            modelBuilder.Entity("BloodBankApp.Models.BloodType", b =>
                {
                    b.Property<Guid>("BloodTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BloodTypeName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("BloodTypeId");

                    b.ToTable("BloodTypes");
                });

            modelBuilder.Entity("BloodBankApp.Models.City", b =>
                {
                    b.Property<Guid>("CityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CityName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("CityId");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("BloodBankApp.Models.DonationPost", b =>
                {
                    b.Property<Guid>("NotificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("AmountRequested")
                        .HasColumnType("float");

                    b.Property<Guid>("BloodTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateRequired")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<Guid>("HospitalId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("PostStatus")
                        .HasColumnType("int");

                    b.HasKey("NotificationId");

                    b.HasIndex("BloodTypeId");

                    b.HasIndex("HospitalId");

                    b.ToTable("DonationPosts");
                });

            modelBuilder.Entity("BloodBankApp.Models.Donor", b =>
                {
                    b.Property<Guid>("DonorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BloodTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<Guid?>("HealthFormQuestionnaireId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("PersonalNumber")
                        .HasColumnType("bigint");

                    b.HasKey("DonorId");

                    b.HasIndex("BloodTypeId");

                    b.HasIndex("CityId");

                    b.ToTable("Donors");
                });

            modelBuilder.Entity("BloodBankApp.Models.HealthFormQuestionnaire", b =>
                {
                    b.Property<Guid>("HealthFormQuestionnaireId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DonorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("datetime2");

                    b.HasKey("HealthFormQuestionnaireId");

                    b.ToTable("HealthFormQuestionnaires");
                });

            modelBuilder.Entity("BloodBankApp.Models.Hospital", b =>
                {
                    b.Property<Guid>("HospitalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ContactNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("HospitalCode")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("HospitalName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<Guid>("LocationId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("HospitalId");

                    b.HasIndex("CityId");

                    b.HasIndex("HospitalCode")
                        .IsUnique();

                    b.HasIndex("LocationId");

                    b.ToTable("Hospitals");
                });

            modelBuilder.Entity("BloodBankApp.Models.Location", b =>
                {
                    b.Property<Guid>("LocationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Latitude")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Longitude")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("LocationId");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("BloodBankApp.Models.MedicalStaff", b =>
                {
                    b.Property<Guid>("MedicalStaffId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("HospitalId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("MedicalStaffId");

                    b.HasIndex("HospitalId");

                    b.ToTable("MedicalStaffs");
                });

            modelBuilder.Entity("BloodBankApp.Models.Message", b =>
                {
                    b.Property<Guid>("MessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<DateTime>("DateSent")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("DonorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("HospitalId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Seen")
                        .HasColumnType("bit");

                    b.Property<int>("Sender")
                        .HasColumnType("int");

                    b.HasKey("MessageId");

                    b.HasIndex("DonorId");

                    b.HasIndex("HospitalId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("BloodBankApp.Models.Notification", b =>
                {
                    b.Property<Guid>("NotificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<Guid>("DonationPostId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("NotificationId");

                    b.HasIndex("DonationPostId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("BloodBankApp.Models.Question", b =>
                {
                    b.Property<Guid>("QuestionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<Guid>("HealthFormQuestionnaireId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("QuestionId");

                    b.HasIndex("HealthFormQuestionnaireId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("BloodBankApp.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("Locked")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("BloodBankApp.Models.BloodDonation", b =>
                {
                    b.HasOne("BloodBankApp.Models.DonationPost", "DonationPost")
                        .WithMany("BloodDonations")
                        .HasForeignKey("DonationPostId")
                        .HasConstraintName("PostDonations")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BloodBankApp.Models.Donor", "Donor")
                        .WithMany("BloodDonations")
                        .HasForeignKey("DonorId")
                        .HasConstraintName("DonorDonations")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("DonationPost");

                    b.Navigation("Donor");
                });

            modelBuilder.Entity("BloodBankApp.Models.BloodReserve", b =>
                {
                    b.HasOne("BloodBankApp.Models.BloodType", "BloodType")
                        .WithMany("BloodReserves")
                        .HasForeignKey("BloodTypeId")
                        .HasConstraintName("TypeReserves")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BloodBankApp.Models.Hospital", "Hospital")
                        .WithMany("BloodReserves")
                        .HasForeignKey("HospitalId")
                        .HasConstraintName("HospitalReserves")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BloodType");

                    b.Navigation("Hospital");
                });

            modelBuilder.Entity("BloodBankApp.Models.DonationPost", b =>
                {
                    b.HasOne("BloodBankApp.Models.BloodType", "BloodType")
                        .WithMany("DonationPosts")
                        .HasForeignKey("BloodTypeId")
                        .HasConstraintName("TypePosts")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BloodBankApp.Models.Hospital", "Hospital")
                        .WithMany("DonationPosts")
                        .HasForeignKey("HospitalId")
                        .HasConstraintName("HospitalPosts")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BloodType");

                    b.Navigation("Hospital");
                });

            modelBuilder.Entity("BloodBankApp.Models.Donor", b =>
                {
                    b.HasOne("BloodBankApp.Models.BloodType", "BloodType")
                        .WithMany("Donors")
                        .HasForeignKey("BloodTypeId")
                        .HasConstraintName("DonorsBloodTypes")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BloodBankApp.Models.City", "City")
                        .WithMany("Donors")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BloodBankApp.Models.User", "User")
                        .WithOne("Donor")
                        .HasForeignKey("BloodBankApp.Models.Donor", "DonorId")
                        .HasConstraintName("UserDonor")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BloodType");

                    b.Navigation("City");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BloodBankApp.Models.HealthFormQuestionnaire", b =>
                {
                    b.HasOne("BloodBankApp.Models.Donor", "Donor")
                        .WithOne("HealthFormQuestionnaire")
                        .HasForeignKey("BloodBankApp.Models.HealthFormQuestionnaire", "HealthFormQuestionnaireId")
                        .HasConstraintName("DonorForms")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Donor");
                });

            modelBuilder.Entity("BloodBankApp.Models.Hospital", b =>
                {
                    b.HasOne("BloodBankApp.Models.City", "City")
                        .WithMany("Hospitals")
                        .HasForeignKey("CityId")
                        .HasConstraintName("CityHospitals")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BloodBankApp.Models.Location", "Location")
                        .WithMany("Hospitals")
                        .HasForeignKey("LocationId")
                        .HasConstraintName("LocationHospitals")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");

                    b.Navigation("Location");
                });

            modelBuilder.Entity("BloodBankApp.Models.MedicalStaff", b =>
                {
                    b.HasOne("BloodBankApp.Models.Hospital", "Hospital")
                        .WithMany("MedicalStaff")
                        .HasForeignKey("HospitalId")
                        .HasConstraintName("MedicalStaffHospital")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BloodBankApp.Models.User", "User")
                        .WithOne("MedicalStaff")
                        .HasForeignKey("BloodBankApp.Models.MedicalStaff", "MedicalStaffId")
                        .HasConstraintName("UserMedicalStaff")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hospital");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BloodBankApp.Models.Message", b =>
                {
                    b.HasOne("BloodBankApp.Models.Donor", "Donor")
                        .WithMany("Messages")
                        .HasForeignKey("DonorId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("BloodBankApp.Models.Hospital", "Hospital")
                        .WithMany("Messages")
                        .HasForeignKey("HospitalId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Donor");

                    b.Navigation("Hospital");
                });

            modelBuilder.Entity("BloodBankApp.Models.Notification", b =>
                {
                    b.HasOne("BloodBankApp.Models.DonationPost", "DonationPost")
                        .WithMany("Notifications")
                        .HasForeignKey("DonationPostId")
                        .HasConstraintName("DonationNotifications")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DonationPost");
                });

            modelBuilder.Entity("BloodBankApp.Models.Question", b =>
                {
                    b.HasOne("BloodBankApp.Models.HealthFormQuestionnaire", "HealthFormQuestionnaire")
                        .WithMany("Questions")
                        .HasForeignKey("HealthFormQuestionnaireId")
                        .HasConstraintName("FormQuestions")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("HealthFormQuestionnaire");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("BloodBankApp.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("BloodBankApp.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BloodBankApp.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("BloodBankApp.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BloodBankApp.Models.BloodType", b =>
                {
                    b.Navigation("BloodReserves");

                    b.Navigation("DonationPosts");

                    b.Navigation("Donors");
                });

            modelBuilder.Entity("BloodBankApp.Models.City", b =>
                {
                    b.Navigation("Donors");

                    b.Navigation("Hospitals");
                });

            modelBuilder.Entity("BloodBankApp.Models.DonationPost", b =>
                {
                    b.Navigation("BloodDonations");

                    b.Navigation("Notifications");
                });

            modelBuilder.Entity("BloodBankApp.Models.Donor", b =>
                {
                    b.Navigation("BloodDonations");

                    b.Navigation("HealthFormQuestionnaire");

                    b.Navigation("Messages");
                });

            modelBuilder.Entity("BloodBankApp.Models.HealthFormQuestionnaire", b =>
                {
                    b.Navigation("Questions");
                });

            modelBuilder.Entity("BloodBankApp.Models.Hospital", b =>
                {
                    b.Navigation("BloodReserves");

                    b.Navigation("DonationPosts");

                    b.Navigation("MedicalStaff");

                    b.Navigation("Messages");
                });

            modelBuilder.Entity("BloodBankApp.Models.Location", b =>
                {
                    b.Navigation("Hospitals");
                });

            modelBuilder.Entity("BloodBankApp.Models.User", b =>
                {
                    b.Navigation("Donor");

                    b.Navigation("MedicalStaff");
                });
#pragma warning restore 612, 618
        }
    }
}
