using Microsoft.EntityFrameworkCore.Migrations;

namespace BaselFinalProjectApp.Migrations
{
    public partial class AddHomeAboutSections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "PersonAbouts");

            migrationBuilder.DropColumn(
                name: "Work",
                table: "PersonAbouts");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "PageAbouts");

            migrationBuilder.DropColumn(
                name: "Footer",
                table: "PageAbouts");

            migrationBuilder.DropColumn(
                name: "Header",
                table: "PageAbouts");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "PageAbouts");

            migrationBuilder.CreateTable(
                name: "PageAboutLanguage",
                columns: table => new
                {
                    PageAboutId = table.Column<int>(nullable: false),
                    LanguageId = table.Column<int>(nullable: false),
                    Header = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Footer = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageAboutLanguage", x => new { x.LanguageId, x.PageAboutId });
                    table.ForeignKey(
                        name: "FK_PageAboutLanguage_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PageAboutLanguage_PageAbouts_PageAboutId",
                        column: x => x.PageAboutId,
                        principalTable: "PageAbouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonAboutLanguage",
                columns: table => new
                {
                    PersonAboutId = table.Column<int>(nullable: false),
                    LanguageId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Work = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonAboutLanguage", x => new { x.LanguageId, x.PersonAboutId });
                    table.ForeignKey(
                        name: "FK_PersonAboutLanguage_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonAboutLanguage_PersonAbouts_PersonAboutId",
                        column: x => x.PersonAboutId,
                        principalTable: "PersonAbouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PageAboutLanguage_PageAboutId",
                table: "PageAboutLanguage",
                column: "PageAboutId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonAboutLanguage_PersonAboutId",
                table: "PersonAboutLanguage",
                column: "PersonAboutId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PageAboutLanguage");

            migrationBuilder.DropTable(
                name: "PersonAboutLanguage");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "PersonAbouts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Work",
                table: "PersonAbouts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "PageAbouts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Footer",
                table: "PageAbouts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Header",
                table: "PageAbouts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "PageAbouts",
                nullable: true);
        }
    }
}
