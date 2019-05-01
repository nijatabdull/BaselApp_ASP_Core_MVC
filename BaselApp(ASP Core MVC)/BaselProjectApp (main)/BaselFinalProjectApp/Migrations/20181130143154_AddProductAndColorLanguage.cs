using Microsoft.EntityFrameworkCore.Migrations;

namespace BaselFinalProjectApp.Migrations
{
    public partial class AddProductAndColorLanguage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "About",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Benefit",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Detail",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Colors");

            migrationBuilder.CreateTable(
                name: "ColorLanguage",
                columns: table => new
                {
                    ColorId = table.Column<int>(nullable: false),
                    LanguageId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColorLanguage", x => new { x.LanguageId, x.ColorId });
                    table.ForeignKey(
                        name: "FK_ColorLanguage_Colors_ColorId",
                        column: x => x.ColorId,
                        principalTable: "Colors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ColorLanguage_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductLanguage",
                columns: table => new
                {
                    ProductId = table.Column<int>(nullable: false),
                    LanguageId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Detail = table.Column<string>(nullable: true),
                    About = table.Column<string>(nullable: true),
                    Benefit = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductLanguage", x => new { x.LanguageId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_ProductLanguage_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductLanguage_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ColorLanguage_ColorId",
                table: "ColorLanguage",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductLanguage_ProductId",
                table: "ProductLanguage",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ColorLanguage");

            migrationBuilder.DropTable(
                name: "ProductLanguage");

            migrationBuilder.AddColumn<string>(
                name: "About",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Benefit",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Detail",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Colors",
                nullable: true);
        }
    }
}
