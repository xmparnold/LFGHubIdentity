using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LFGHub.Data.Migrations
{
    public partial class UpdatedFKs2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "UserId",
                table: "Posts");

            migrationBuilder.AddForeignKey(
                name: "Id",
                table: "Posts",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Id",
                table: "Posts");

            migrationBuilder.AddForeignKey(
                name: "UserId",
                table: "Posts",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
