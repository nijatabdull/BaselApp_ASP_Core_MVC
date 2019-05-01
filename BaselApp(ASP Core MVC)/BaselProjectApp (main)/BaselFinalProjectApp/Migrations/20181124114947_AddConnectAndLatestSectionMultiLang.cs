using Microsoft.EntityFrameworkCore.Migrations;

namespace BaselFinalProjectApp.Migrations
{
    public partial class AddConnectAndLatestSectionMultiLang : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "ContactAbouts");

            migrationBuilder.DropColumn(
                name: "Footer",
                table: "ContactAbouts");

            migrationBuilder.DropColumn(
                name: "Header",
                table: "ContactAbouts");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "ContactAbouts",
                newName: "Image");

            migrationBuilder.CreateTable(
                name: "ContactAboutLanguage",
                columns: table => new
                {
                    Header = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Footer = table.Column<string>(nullable: true),
                    ContactAboutId = table.Column<int>(nullable: false),
                    LanguageId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactAboutLanguage", x => new { x.LanguageId, x.ContactAboutId });
                    table.ForeignKey(
                        name: "FK_ContactAboutLanguage_ContactAbouts_ContactAboutId",
                        column: x => x.ContactAboutId,
                        principalTable: "ContactAbouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContactAboutLanguage_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LatestNewsLanguage",
                columns: table => new
                {
                    LatestNewsId = table.Column<int>(nullable: false),
                    LanguageId = table.Column<int>(nullable: false),
                    Header = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Footer = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LatestNewsLanguage", x => new { x.LanguageId, x.LatestNewsId });
                    table.ForeignKey(
                        name: "FK_LatestNewsLanguage_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LatestNewsLanguage_LatesNews_LatestNewsId",
                        column: x => x.LatestNewsId,
                        principalTable: "LatesNews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContactAboutLanguage_ContactAboutId",
                table: "ContactAboutLanguage",
                column: "ContactAboutId");

            migrationBuilder.CreateIndex(
                name: "IX_LatestNewsLanguage_LatestNewsId",
                table: "LatestNewsLanguage",
                column: "LatestNewsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactAboutLanguage");

            migrationBuilder.DropTable(
                name: "LatestNewsLanguage");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "ContactAbouts",
                newName: "Title");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ContactAbouts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Footer",
                table: "ContactAbouts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Header",
                table: "ContactAbouts",
                nullable: true);
        }
    }
}
