using Microsoft.EntityFrameworkCore.Migrations;

namespace BaselFinalProjectApp.Migrations
{
    public partial class ChangeOrderProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Company",
                table: "BillingDetails");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "BillingDetails",
                newName: "Apartment");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Apartment",
                table: "BillingDetails",
                newName: "Country");

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "BillingDetails",
                nullable: true);
        }
    }
}
