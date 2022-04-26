using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class AddLikeChancges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Users_LikeUserId",
                table: "Likes");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Users_LikeUserId",
                table: "Likes",
                column: "LikeUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Users_LikeUserId",
                table: "Likes");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Users_LikeUserId",
                table: "Likes",
                column: "LikeUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
