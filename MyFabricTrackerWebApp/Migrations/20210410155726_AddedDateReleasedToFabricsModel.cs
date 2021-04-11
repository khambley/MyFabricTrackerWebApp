using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFabricTrackerWebApp.Migrations
{
    public partial class AddedDateReleasedToFabricsModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateReleased",
                table: "Fabrics",
                type: "date",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateReleased",
                table: "Fabrics");
        }
    }
}
