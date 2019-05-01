using Microsoft.EntityFrameworkCore.Migrations;

namespace BaselFinalProjectApp.Migrations
{
    public partial class AddConnectAndLatestSectionMultiLang2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "LatesNews");

            migrationBuilder.DropColumn(
                name: "Footer",
                table: "LatesNews");

            migrationBuilder.DropColumn(
                name: "Header",
                table: "LatesNews");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "LatesNews");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "LatesNews",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Footer",
                table: "LatesNews",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Header",
                table: "LatesNews",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "LatesNews",
                nullable: true);
        }
    }
}
