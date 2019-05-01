using Microsoft.EntityFrameworkCore.Migrations;

namespace BaselFinalProjectApp.Migrations
{
    public partial class addCategoryLanguage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Categories",
                newName: "Controller");

            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "Categories",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Action",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "Controller",
                table: "Categories",
                newName: "Name");
        }
    }
}
