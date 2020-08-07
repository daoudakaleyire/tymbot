using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tymbot.Migrations
{
    public partial class UserChatId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFriend_UserTimeZone_UserId",
                table: "UserFriend");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTimeZone",
                table: "UserTimeZone");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "UserTimeZone",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<long>(
                name: "ChatId",
                table: "UserTimeZone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "UserTimeZone",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "FriendId",
                table: "UserFriend",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "UserFriend",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

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

            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "UserTimeZone");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "UserTimeZone");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UserTimeZone",
                type: "int",
                nullable: false,
                oldClrType: typeof(long))
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "FriendId",
                table: "UserFriend",
                type: "int",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UserFriend",
                type: "int",
                nullable: false,
                oldClrType: typeof(long));

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
