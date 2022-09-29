using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BloodBankApp.Migrations
{
    public partial class CodeTablee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Codes",
                columns: table => new
                {
                    CodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CodeValue = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Codes", x => x.CodeId);
                    table.ForeignKey(
                        name: "CodeDonor",
                        column: x => x.CodeId,
                        principalTable: "Donors",
                        principalColumn: "DonorId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Codes");
        }
    }
}
