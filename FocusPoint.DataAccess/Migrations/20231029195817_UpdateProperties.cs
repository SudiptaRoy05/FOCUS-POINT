using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FocusPointWeb.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "MediaItems");

            migrationBuilder.AddColumn<int>(
                name: "NormalUserId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PhotographerId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NormalUserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PhotographerId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "MediaItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
