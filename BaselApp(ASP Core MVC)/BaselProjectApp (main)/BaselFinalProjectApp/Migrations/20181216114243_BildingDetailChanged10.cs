using Microsoft.EntityFrameworkCore.Migrations;

namespace BaselFinalProjectApp.Migrations
{
    public partial class BildingDetailChanged10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillingDetails_Orders_OrderId",
                table: "BillingDetails");

            migrationBuilder.DropIndex(
                name: "IX_BillingDetails_OrderId",
                table: "BillingDetails");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "BillingDetails");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "BillingDetails",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BillingDetails_OrderId",
                table: "BillingDetails",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_BillingDetails_Orders_OrderId",
                table: "BillingDetails",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
