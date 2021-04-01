using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFabricTrackerWebApp.Migrations
{
    public partial class AddedSourceTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FabricSourceName",
                table: "Fabrics");

            migrationBuilder.DropColumn(
                name: "FabricSourceUrl",
                table: "Fabrics");

            migrationBuilder.AddColumn<int>(
                name: "SourceId",
                table: "Fabrics",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Sources",
                columns: table => new
                {
                    SourceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceName = table.Column<string>(nullable: true),
                    WebsiteUrl = table.Column<string>(nullable: true),
                    StreetAddress = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sources", x => x.SourceId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fabrics_SourceId",
                table: "Fabrics",
                column: "SourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fabrics_Sources_SourceId",
                table: "Fabrics",
                column: "SourceId",
                principalTable: "Sources",
                principalColumn: "SourceId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fabrics_Sources_SourceId",
                table: "Fabrics");

            migrationBuilder.DropTable(
                name: "Sources");

            migrationBuilder.DropIndex(
                name: "IX_Fabrics_SourceId",
                table: "Fabrics");

            migrationBuilder.DropColumn(
                name: "SourceId",
                table: "Fabrics");

            migrationBuilder.AddColumn<string>(
                name: "FabricSourceName",
                table: "Fabrics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FabricSourceUrl",
                table: "Fabrics",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
