using Microsoft.EntityFrameworkCore.Migrations;

namespace BaselFinalProjectApp.Migrations
{
    public partial class AddSlideImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "SlideLanguage");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Slides",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Slides");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "SlideLanguage",
                nullable: true);
        }
    }
}
