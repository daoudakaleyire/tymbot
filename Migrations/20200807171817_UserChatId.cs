using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tymbot.Migrations
{
    public partial class UserChatId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
