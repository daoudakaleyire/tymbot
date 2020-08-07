using Microsoft.EntityFrameworkCore.Migrations;

namespace tymbot.Migrations
{
    public partial class RemoveUserFriendKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFriend",
                table: "UserFriend");

            migrationBuilder.CreateIndex(
                name: "IX_UserFriend_UserId",
                table: "UserFriend",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserFriend_UserId",
                table: "UserFriend");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFriend",
                table: "UserFriend",
                columns: new[] { "UserId", "FriendId" });
        }
    }
}
