using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class Pozadeprofil : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PhotoId",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhotoId",
                table: "Users",
                column: "PhotoId",
                unique: true,
                filter: "[PhotoId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "USER_PROFILE_PHOTO_FK",
                table: "Users",
                column: "PhotoId",
                principalTable: "Photo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "USER_PROFILE_PHOTO_FK",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_PhotoId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "Users");
        }
    }
}
