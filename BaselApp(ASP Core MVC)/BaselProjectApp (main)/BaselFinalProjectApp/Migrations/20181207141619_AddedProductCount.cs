using Microsoft.EntityFrameworkCore.Migrations;

namespace BaselFinalProjectApp.Migrations
{
    public partial class AddedProductCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "Carts");

            migrationBuilder.AddColumn<int>(
                name: "ProductCount",
                table: "ProductCart",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductCount",
                table: "ProductCart");

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "Carts",
                nullable: false,
                defaultValue: 0);
        }
    }
}
