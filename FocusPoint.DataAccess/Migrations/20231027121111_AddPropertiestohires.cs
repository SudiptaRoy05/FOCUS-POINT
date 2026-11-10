using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FocusPointWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertiestohires : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Hires",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Hires_ApplicationUserId",
                table: "Hires",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hires_AspNetUsers_ApplicationUserId",
                table: "Hires",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hires_AspNetUsers_ApplicationUserId",
                table: "Hires");

            migrationBuilder.DropIndex(
                name: "IX_Hires_ApplicationUserId",
                table: "Hires");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Hires");
        }
    }
}
