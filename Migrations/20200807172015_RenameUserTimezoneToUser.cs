using Microsoft.EntityFrameworkCore.Migrations;

namespace tymbot.Migrations
{
    public partial class RenameUserTimezoneToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFriend_UserTimeZone_UserId",
                table: "UserFriend");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTimeZone",
                table: "UserTimeZone");

            migrationBuilder.RenameTable(
                name: "UserTimeZone",
                newName: "User");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFriend_User_UserId",
                table: "UserFriend",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFriend_User_UserId",
                table: "UserFriend");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "UserTimeZone");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTimeZone",
                table: "UserTimeZone",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFriend_UserTimeZone_UserId",
                table: "UserFriend",
                column: "UserId",
                principalTable: "UserTimeZone",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
