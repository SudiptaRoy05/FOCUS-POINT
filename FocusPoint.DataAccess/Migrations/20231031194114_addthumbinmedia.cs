using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FocusPointWeb.Migrations
{
    /// <inheritdoc />
    public partial class addthumbinmedia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Thumbnail1",
                table: "MediaItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Thumbnail2",
                table: "MediaItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Thumbnail1",
                table: "MediaItems");

            migrationBuilder.DropColumn(
                name: "Thumbnail2",
                table: "MediaItems");
        }
    }
}
