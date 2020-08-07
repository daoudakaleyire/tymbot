﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace tymbot.Migrations
{
    public partial class UserFriendFrientNavigation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserFriend_FriendId",
                table: "UserFriend",
                column: "FriendId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFriend_User_FriendId",
                table: "UserFriend",
                column: "FriendId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFriend_User_FriendId",
                table: "UserFriend");

            migrationBuilder.DropIndex(
                name: "IX_UserFriend_FriendId",
                table: "UserFriend");
        }
    }
}