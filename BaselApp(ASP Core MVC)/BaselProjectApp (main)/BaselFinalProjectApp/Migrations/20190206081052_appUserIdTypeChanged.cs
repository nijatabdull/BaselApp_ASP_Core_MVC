using Microsoft.EntityFrameworkCore.Migrations;

namespace BaselFinalProjectApp.Migrations
{
    public partial class appUserIdTypeChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {       
            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "WishLists",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_WishLists_AppUserId",
                table: "WishLists",
                column: "AppUserId",
                unique: true,
                filter: "[AppUserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_WishLists_AspNetUsers_AppUserId",
                table: "WishLists",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WishLists_AspNetUsers_AppUserId",
                table: "WishLists");

            migrationBuilder.DropIndex(
                name: "IX_WishLists_AppUserId",
                table: "WishLists");

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "WishLists",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_WishListId",
                table: "AspNetUsers",
                column: "WishListId",
                unique: true,
                filter: "[WishListId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_WishLists_WishListId",
                table: "AspNetUsers",
                column: "WishListId",
                principalTable: "WishLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
