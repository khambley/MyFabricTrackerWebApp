using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFabricTrackerWebApp.Migrations
{
    public partial class AdditionalFabricProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccentColor1",
                table: "Fabrics",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccentColor2",
                table: "Fabrics",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccentColor3",
                table: "Fabrics",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BackgroundColor",
                table: "Fabrics",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FabricWidth",
                table: "Fabrics",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDiscontinued",
                table: "Fabrics",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPopular",
                table: "Fabrics",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_FabricId",
                table: "Transactions",
                column: "FabricId");

            migrationBuilder.CreateIndex(
                name: "IX_Fabrics_MainCategoryId",
                table: "Fabrics",
                column: "MainCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Fabrics_SubCategoryId",
                table: "Fabrics",
                column: "SubCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fabrics_MainCategories_MainCategoryId",
                table: "Fabrics",
                column: "MainCategoryId",
                principalTable: "MainCategories",
                principalColumn: "MainCategoryId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Fabrics_SubCategories_SubCategoryId",
                table: "Fabrics",
                column: "SubCategoryId",
                principalTable: "SubCategories",
                principalColumn: "SubCategoryId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Fabrics_FabricId",
                table: "Transactions",
                column: "FabricId",
                principalTable: "Fabrics",
                principalColumn: "FabricID",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fabrics_MainCategories_MainCategoryId",
                table: "Fabrics");

            migrationBuilder.DropForeignKey(
                name: "FK_Fabrics_SubCategories_SubCategoryId",
                table: "Fabrics");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Fabrics_FabricId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_FabricId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Fabrics_MainCategoryId",
                table: "Fabrics");

            migrationBuilder.DropIndex(
                name: "IX_Fabrics_SubCategoryId",
                table: "Fabrics");

            migrationBuilder.DropColumn(
                name: "AccentColor1",
                table: "Fabrics");

            migrationBuilder.DropColumn(
                name: "AccentColor2",
                table: "Fabrics");

            migrationBuilder.DropColumn(
                name: "AccentColor3",
                table: "Fabrics");

            migrationBuilder.DropColumn(
                name: "BackgroundColor",
                table: "Fabrics");

            migrationBuilder.DropColumn(
                name: "FabricWidth",
                table: "Fabrics");

            migrationBuilder.DropColumn(
                name: "IsDiscontinued",
                table: "Fabrics");

            migrationBuilder.DropColumn(
                name: "IsPopular",
                table: "Fabrics");
        }
    }
}
