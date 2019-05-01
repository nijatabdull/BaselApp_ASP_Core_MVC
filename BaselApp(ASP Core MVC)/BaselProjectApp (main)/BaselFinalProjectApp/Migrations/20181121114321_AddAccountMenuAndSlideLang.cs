using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BaselFinalProjectApp.Migrations
{
    public partial class AddAccountMenuAndSlideLang : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoginRegisters");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Slides");

            migrationBuilder.DropColumn(
                name: "Footer",
                table: "Slides");

            migrationBuilder.DropColumn(
                name: "Header",
                table: "Slides");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Slides");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Slides");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AccountMenus");

            migrationBuilder.CreateTable(
                name: "AccountMenuLanguage",
                columns: table => new
                {
                    AccountMenuId = table.Column<int>(nullable: false),
                    LanguageId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountMenuLanguage", x => new { x.LanguageId, x.AccountMenuId });
                    table.ForeignKey(
                        name: "FK_AccountMenuLanguage_AccountMenus_AccountMenuId",
                        column: x => x.AccountMenuId,
                        principalTable: "AccountMenus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountMenuLanguage_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SlideLanguage",
                columns: table => new
                {
                    SlideId = table.Column<int>(nullable: false),
                    LanguageId = table.Column<int>(nullable: false),
                    Header = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Footer = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SlideLanguage", x => new { x.LanguageId, x.SlideId });
                    table.ForeignKey(
                        name: "FK_SlideLanguage_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SlideLanguage_Slides_SlideId",
                        column: x => x.SlideId,
                        principalTable: "Slides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountMenuLanguage_AccountMenuId",
                table: "AccountMenuLanguage",
                column: "AccountMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_SlideLanguage_SlideId",
                table: "SlideLanguage",
                column: "SlideId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountMenuLanguage");

            migrationBuilder.DropTable(
                name: "SlideLanguage");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Slides",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Footer",
                table: "Slides",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Header",
                table: "Slides",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Slides",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Slides",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AccountMenus",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LoginRegisters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Header = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginRegisters", x => x.Id);
                });
        }
    }
}
