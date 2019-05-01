using Microsoft.EntityFrameworkCore.Migrations;

namespace BaselFinalProjectApp.Migrations
{
    public partial class OrderProductCountAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductCount",
                table: "ProductOrder",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductCount",
                table: "ProductOrder");
        }
    }
}
