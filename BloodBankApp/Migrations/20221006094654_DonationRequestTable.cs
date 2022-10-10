using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BloodBankApp.Migrations
{
    public partial class DonationRequestTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DonationRequest",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DonationPostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DonorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonationRequest", x => x.Id);
                    table.ForeignKey(
                        name: "DonorReqDonations",
                        column: x => x.DonorId,
                        principalTable: "Donors",
                        principalColumn: "DonorId");
                    table.ForeignKey(
                        name: "FK_DonationRequest_DonationPosts_DonationPostId",
                        column: x => x.DonationPostId,
                        principalTable: "DonationPosts",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DonationRequest_DonationPostId",
                table: "DonationRequest",
                column: "DonationPostId");

            migrationBuilder.CreateIndex(
                name: "IX_DonationRequest_DonorId",
                table: "DonationRequest",
                column: "DonorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "DonorReqDonations",
                table: "BloodDonations");

            migrationBuilder.DropTable(
                name: "DonationRequest");
        }
    }
}
