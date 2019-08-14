using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class Commentsaremadeonlybymembersandpostshaveasinglephotoandtext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "PHOTO_POST_FK",
                table: "Photo");

            migrationBuilder.DropIndex(
                name: "IX_Photo_PostId",
                table: "Photo");

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "Post",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Comment",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Photo_PostId",
                table: "Photo",
                column: "PostId",
                unique: true,
                filter: "[PostId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Photo_Post_PostId",
                table: "Photo",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photo_Post_PostId",
                table: "Photo");

            migrationBuilder.DropIndex(
                name: "IX_Photo_PostId",
                table: "Photo");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "Post");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Comment",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Photo_PostId",
                table: "Photo",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "PHOTO_POST_FK",
                table: "Photo",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
