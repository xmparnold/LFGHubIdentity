using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LFGHub.Data.Migrations
{
    public partial class UpdatedGameActivities4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameActivities_AspNetUsers_SuggestedById",
                table: "GameActivities");

            migrationBuilder.DropIndex(
                name: "IX_GameActivities_SuggestedById",
                table: "GameActivities");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GameActivities");

            migrationBuilder.DropColumn(
                name: "SuggestedById",
                table: "GameActivities");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "GameActivities",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SuggestedById",
                table: "GameActivities",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameActivities_SuggestedById",
                table: "GameActivities",
                column: "SuggestedById");

            migrationBuilder.AddForeignKey(
                name: "FK_GameActivities_AspNetUsers_SuggestedById",
                table: "GameActivities",
                column: "SuggestedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
