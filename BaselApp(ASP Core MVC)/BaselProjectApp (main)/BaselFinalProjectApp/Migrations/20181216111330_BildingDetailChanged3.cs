using Microsoft.EntityFrameworkCore.Migrations;

namespace BaselFinalProjectApp.Migrations
{
    public partial class BildingDetailChanged3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_BillingDetails_BillingDetailId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BillingDetailId",
                table: "AspNetUsers");

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
                onDelete: ReferentialAction.SetDefault);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_BillingDetails_BillingDetailId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BillingDetailId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "BillingDetailId",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BillingDetailId",
                table: "AspNetUsers",
                column: "BillingDetailId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_BillingDetails_BillingDetailId",
                table: "AspNetUsers",
                column: "BillingDetailId",
                principalTable: "BillingDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
