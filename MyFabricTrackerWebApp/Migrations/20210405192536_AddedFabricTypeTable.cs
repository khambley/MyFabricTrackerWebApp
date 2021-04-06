using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFabricTrackerWebApp.Migrations
{
    public partial class AddedFabricTypeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FabricType",
                table: "Fabrics");

            migrationBuilder.AddColumn<int>(
                name: "FabricTypeId",
                table: "Fabrics",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FabricType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FabricType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fabrics_FabricTypeId",
                table: "Fabrics",
                column: "FabricTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fabrics_FabricType_FabricTypeId",
                table: "Fabrics",
                column: "FabricTypeId",
                principalTable: "FabricType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fabrics_FabricType_FabricTypeId",
                table: "Fabrics");

            migrationBuilder.DropTable(
                name: "FabricType");

            migrationBuilder.DropIndex(
                name: "IX_Fabrics_FabricTypeId",
                table: "Fabrics");

            migrationBuilder.DropColumn(
                name: "FabricTypeId",
                table: "Fabrics");

            migrationBuilder.AddColumn<string>(
                name: "FabricType",
                table: "Fabrics",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
