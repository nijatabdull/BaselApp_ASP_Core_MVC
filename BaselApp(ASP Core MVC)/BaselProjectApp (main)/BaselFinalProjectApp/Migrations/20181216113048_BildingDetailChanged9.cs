using Microsoft.EntityFrameworkCore.Migrations;

namespace BaselFinalProjectApp.Migrations
{
    public partial class BildingDetailChanged9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_BillingDetails_BillingDetailId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "BillingDetailId",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_BillingDetails_BillingDetailId",
                table: "AspNetUsers",
                column: "BillingDetailId",
                principalTable: "BillingDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_BillingDetails_BillingDetailId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "BillingDetailId",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_BillingDetails_BillingDetailId",
                table: "AspNetUsers",
                column: "BillingDetailId",
                principalTable: "BillingDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
