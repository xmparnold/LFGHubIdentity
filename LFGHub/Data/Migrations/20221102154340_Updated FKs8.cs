using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LFGHub.Data.Migrations
{
    public partial class UpdatedFKs8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_Posts_PostId1",
                table: "GroupMembers");

            migrationBuilder.DropIndex(
                name: "IX_GroupMembers_PostId1",
                table: "GroupMembers");

            migrationBuilder.DropColumn(
                name: "PostId1",
                table: "GroupMembers");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "GroupMembers",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembers_PostId",
                table: "GroupMembers",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembers_Posts_PostId",
                table: "GroupMembers",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_Posts_PostId",
                table: "GroupMembers");

            migrationBuilder.DropIndex(
                name: "IX_GroupMembers_PostId",
                table: "GroupMembers");

            migrationBuilder.AlterColumn<string>(
                name: "PostId",
                table: "GroupMembers",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "PostId1",
                table: "GroupMembers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembers_PostId1",
                table: "GroupMembers",
                column: "PostId1");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembers_Posts_PostId1",
                table: "GroupMembers",
                column: "PostId1",
                principalTable: "Posts",
                principalColumn: "PostId");
        }
    }
}
