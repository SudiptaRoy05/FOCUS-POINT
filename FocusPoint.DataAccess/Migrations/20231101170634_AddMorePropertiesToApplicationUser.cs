using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FocusPointWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddMorePropertiesToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialLink1",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialLink2",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialLink3",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bio",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SocialLink1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SocialLink2",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SocialLink3",
                table: "AspNetUsers");
        }
    }
}
