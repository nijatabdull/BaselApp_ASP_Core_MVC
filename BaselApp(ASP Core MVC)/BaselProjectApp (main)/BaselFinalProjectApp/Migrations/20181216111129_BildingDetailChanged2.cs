using Microsoft.EntityFrameworkCore.Migrations;

namespace BaselFinalProjectApp.Migrations
{
    public partial class BildingDetailChanged2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BillingDetailId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "BillingDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BillingDetailId",
                table: "AspNetUsers",
                column: "BillingDetailId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BillingDetailId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "BillingDetails");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BillingDetailId",
                table: "AspNetUsers",
                column: "BillingDetailId");
        }
    }
}
