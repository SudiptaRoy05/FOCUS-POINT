using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FocusPointWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddAnotherPropertyToHire : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PackegesId",
                table: "Hires",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Hires_PackegesId",
                table: "Hires",
                column: "PackegesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hires_Packeges_PackegesId",
                table: "Hires",
                column: "PackegesId",
                principalTable: "Packeges",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hires_Packeges_PackegesId",
                table: "Hires");

            migrationBuilder.DropIndex(
                name: "IX_Hires_PackegesId",
                table: "Hires");

            migrationBuilder.DropColumn(
                name: "PackegesId",
                table: "Hires");
        }
    }
}
