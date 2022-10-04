using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BloodBankApp.Migrations
{
    public partial class BloodDonation_HospitalId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "DonationPostId",
                table: "BloodDonations",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "HospitalId",
                table: "BloodDonations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BloodDonations_HospitalId",
                table: "BloodDonations",
                column: "HospitalId");

            migrationBuilder.AddForeignKey(
                name: "FK_BloodDonations_Hospitals_HospitalId",
                table: "BloodDonations",
                column: "HospitalId",
                principalTable: "Hospitals",
                principalColumn: "HospitalId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BloodDonations_Hospitals_HospitalId",
                table: "BloodDonations");

            migrationBuilder.DropIndex(
                name: "IX_BloodDonations_HospitalId",
                table: "BloodDonations");

            migrationBuilder.DropColumn(
                name: "HospitalId",
                table: "BloodDonations");

            migrationBuilder.AlterColumn<Guid>(
                name: "DonationPostId",
                table: "BloodDonations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }
    }
}
