using Microsoft.EntityFrameworkCore.Migrations;

namespace BaselFinalProjectApp.Migrations
{
    public partial class BildingDetailChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_BillingDetails_BillingDetailId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_BillingDetailId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BillingDetailId",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "BillingDetails",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BillingDetailId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BillingDetails_OrderId",
                table: "BillingDetails",
                column: "OrderId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_BillingDetails_Orders_OrderId",
                table: "BillingDetails",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_BillingDetails_BillingDetailId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_BillingDetails_Orders_OrderId",
                table: "BillingDetails");

            migrationBuilder.DropIndex(
                name: "IX_BillingDetails_OrderId",
                table: "BillingDetails");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BillingDetailId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "BillingDetails");

            migrationBuilder.DropColumn(
                name: "BillingDetailId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "BillingDetailId",
                table: "Orders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BillingDetailId",
                table: "Orders",
                column: "BillingDetailId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_BillingDetails_BillingDetailId",
                table: "Orders",
                column: "BillingDetailId",
                principalTable: "BillingDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
