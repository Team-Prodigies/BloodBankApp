using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BloodBankApp.Migrations
{
    public partial class LockedAttributeAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "DonationsPosts",
                table: "BloodDonations");

            migrationBuilder.DropForeignKey(
                name: "HospitalDonations",
                table: "DonationPosts");

            migrationBuilder.DropForeignKey(
                name: "PostTypes",
                table: "DonationPosts");

            migrationBuilder.DropForeignKey(
                name: "DonorsTypes",
                table: "Donors");

            migrationBuilder.DropForeignKey(
                name: "UserDonorr",
                table: "Donors");

            migrationBuilder.DropForeignKey(
                name: "DonorrForms",
                table: "HealthFormQuestionnaires");

            migrationBuilder.DropForeignKey(
                name: "DonorsLocationn",
                table: "Hospitals");

            migrationBuilder.DropForeignKey(
                name: "MedicalHospital",
                table: "MedicalStaffs");

            migrationBuilder.DropForeignKey(
                name: "UserMedicalStafff",
                table: "MedicalStaffs");

            migrationBuilder.DropForeignKey(
                name: "donationnotif",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "formqst",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "UserSuperAdminn",
                table: "SuperAdmins");

            migrationBuilder.AlterColumn<Guid>(
                name: "HealthFormQuestionnaireId",
                table: "Donors",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<bool>(
                name: "Locked",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "PostDonations",
                table: "BloodDonations",
                column: "DonationPostId",
                principalTable: "DonationPosts",
                principalColumn: "NotificationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "HospitalPosts",
                table: "DonationPosts",
                column: "HospitalId",
                principalTable: "Hospitals",
                principalColumn: "HospitalId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "TypePosts",
                table: "DonationPosts",
                column: "BloodTypeId",
                principalTable: "BloodTypes",
                principalColumn: "BloodTypeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "DonorsBloodTypes",
                table: "Donors",
                column: "BloodTypeId",
                principalTable: "BloodTypes",
                principalColumn: "BloodTypeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "UserDonor",
                table: "Donors",
                column: "DonorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "DonorForms",
                table: "HealthFormQuestionnaires",
                column: "HealthFormQuestionnaireId",
                principalTable: "Donors",
                principalColumn: "DonorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "LocationHospitals",
                table: "Hospitals",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "MedicalStaffHospital",
                table: "MedicalStaffs",
                column: "HospitalId",
                principalTable: "Hospitals",
                principalColumn: "HospitalId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "UserMedicalStaff",
                table: "MedicalStaffs",
                column: "MedicalStaffId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "DonationNotifications",
                table: "Notifications",
                column: "DonationPostId",
                principalTable: "DonationPosts",
                principalColumn: "NotificationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FormQuestions",
                table: "Questions",
                column: "HealthFormQuestionnaireId",
                principalTable: "HealthFormQuestionnaires",
                principalColumn: "HealthFormQuestionnaireId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "UserSuperAdmin",
                table: "SuperAdmins",
                column: "SuperAdminId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "PostDonations",
                table: "BloodDonations");

            migrationBuilder.DropForeignKey(
                name: "HospitalPosts",
                table: "DonationPosts");

            migrationBuilder.DropForeignKey(
                name: "TypePosts",
                table: "DonationPosts");

            migrationBuilder.DropForeignKey(
                name: "DonorsBloodTypes",
                table: "Donors");

            migrationBuilder.DropForeignKey(
                name: "UserDonor",
                table: "Donors");

            migrationBuilder.DropForeignKey(
                name: "DonorForms",
                table: "HealthFormQuestionnaires");

            migrationBuilder.DropForeignKey(
                name: "LocationHospitals",
                table: "Hospitals");

            migrationBuilder.DropForeignKey(
                name: "MedicalStaffHospital",
                table: "MedicalStaffs");

            migrationBuilder.DropForeignKey(
                name: "UserMedicalStaff",
                table: "MedicalStaffs");

            migrationBuilder.DropForeignKey(
                name: "DonationNotifications",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FormQuestions",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "UserSuperAdmin",
                table: "SuperAdmins");

            migrationBuilder.DropColumn(
                name: "Locked",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<Guid>(
                name: "HealthFormQuestionnaireId",
                table: "Donors",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "DonationsPosts",
                table: "BloodDonations",
                column: "DonationPostId",
                principalTable: "DonationPosts",
                principalColumn: "NotificationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "HospitalDonations",
                table: "DonationPosts",
                column: "HospitalId",
                principalTable: "Hospitals",
                principalColumn: "HospitalId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "PostTypes",
                table: "DonationPosts",
                column: "BloodTypeId",
                principalTable: "BloodTypes",
                principalColumn: "BloodTypeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "DonorsTypes",
                table: "Donors",
                column: "BloodTypeId",
                principalTable: "BloodTypes",
                principalColumn: "BloodTypeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "UserDonorr",
                table: "Donors",
                column: "DonorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "DonorrForms",
                table: "HealthFormQuestionnaires",
                column: "HealthFormQuestionnaireId",
                principalTable: "Donors",
                principalColumn: "DonorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "DonorsLocationn",
                table: "Hospitals",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "MedicalHospital",
                table: "MedicalStaffs",
                column: "HospitalId",
                principalTable: "Hospitals",
                principalColumn: "HospitalId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "UserMedicalStafff",
                table: "MedicalStaffs",
                column: "MedicalStaffId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "donationnotif",
                table: "Notifications",
                column: "DonationPostId",
                principalTable: "DonationPosts",
                principalColumn: "NotificationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "formqst",
                table: "Questions",
                column: "HealthFormQuestionnaireId",
                principalTable: "HealthFormQuestionnaires",
                principalColumn: "HealthFormQuestionnaireId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "UserSuperAdminn",
                table: "SuperAdmins",
                column: "SuperAdminId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
