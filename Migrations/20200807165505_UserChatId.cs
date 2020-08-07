using Microsoft.EntityFrameworkCore.Migrations;

namespace tymbot.Migrations
{
    public partial class UserChatId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFriend_User_UserId",
                table: "UserFriend");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "UserTimezone");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTimezone",
                table: "UserTimezone",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFriend_UserTimezone_UserId",
                table: "UserFriend",
                column: "UserId",
                principalTable: "UserTimezone",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFriend_UserTimezone_UserId",
                table: "UserFriend");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTimezone",
                table: "UserTimezone");

            migrationBuilder.RenameTable(
                name: "UserTimezone",
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
    }
}
