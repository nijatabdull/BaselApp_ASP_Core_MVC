using Microsoft.EntityFrameworkCore.Migrations;

namespace BaselFinalProjectApp.Migrations
{
    public partial class BildingDetailChanged4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_BillingDetails_BillingDetailId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BillingDetailId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "BillingDetails");

            migrationBuilder.AlterColumn<int>(
                name: "BillingDetailId",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BillingDetailId",
                table: "AspNetUsers",
                column: "BillingDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_BillingDetails_BillingDetailId",
                table: "AspNetUsers",
                column: "BillingDetailId",
                principalTable: "BillingDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_BillingDetails_BillingDetailId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BillingDetailId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "BillingDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "BillingDetailId",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BillingDetailId",
                table: "AspNetUsers",
                column: "BillingDetailId",
                unique: true,
                filter: "[BillingDetailId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_BillingDetails_BillingDetailId",
                table: "AspNetUsers",
                column: "BillingDetailId",
                principalTable: "BillingDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
