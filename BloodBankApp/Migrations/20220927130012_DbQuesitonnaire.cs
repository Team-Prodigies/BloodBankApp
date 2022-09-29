using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BloodBankApp.Migrations
{
    public partial class DbQuesitonnaire : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "DonorForms",
                table: "HealthFormQuestionnaires");

            migrationBuilder.DropColumn(
                name: "DonorId",
                table: "HealthFormQuestionnaires");

            migrationBuilder.AddColumn<int>(
                name: "Answer",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Donors_HealthFormQuestionnaireId",
                table: "Donors",
                column: "HealthFormQuestionnaireId");

            migrationBuilder.AddForeignKey(
                name: "FK_Donors_HealthFormQuestionnaires_HealthFormQuestionnaireId",
                table: "Donors",
                column: "HealthFormQuestionnaireId",
                principalTable: "HealthFormQuestionnaires",
                principalColumn: "HealthFormQuestionnaireId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donors_HealthFormQuestionnaires_HealthFormQuestionnaireId",
                table: "Donors");

            migrationBuilder.DropIndex(
                name: "IX_Donors_HealthFormQuestionnaireId",
                table: "Donors");

            migrationBuilder.DropColumn(
                name: "Answer",
                table: "Questions");

            migrationBuilder.AddColumn<Guid>(
                name: "DonorId",
                table: "HealthFormQuestionnaires",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "DonorForms",
                table: "HealthFormQuestionnaires",
                column: "HealthFormQuestionnaireId",
                principalTable: "Donors",
                principalColumn: "DonorId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
