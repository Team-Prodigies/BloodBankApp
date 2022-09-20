using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BloodBankApp.Migrations
{
    public partial class Messagestablefix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_ReceiverId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_SenderId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ReceiverId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_SenderId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ReceiverId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "Messages");

            migrationBuilder.AddColumn<Guid>(
                name: "DonorId",
                table: "Messages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "HospitalId",
                table: "Messages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Sender",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_DonorId",
                table: "Messages",
                column: "DonorId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_HospitalId",
                table: "Messages",
                column: "HospitalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Donors_DonorId",
                table: "Messages",
                column: "DonorId",
                principalTable: "Donors",
                principalColumn: "DonorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Hospitals_HospitalId",
                table: "Messages",
                column: "HospitalId",
                principalTable: "Hospitals",
                principalColumn: "HospitalId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Donors_DonorId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Hospitals_HospitalId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_DonorId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_HospitalId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "DonorId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "HospitalId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Sender",
                table: "Messages");

            migrationBuilder.AddColumn<Guid>(
                name: "ReceiverId",
                table: "Messages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SenderId",
                table: "Messages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ReceiverId",
                table: "Messages",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_ReceiverId",
                table: "Messages",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_SenderId",
                table: "Messages",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
