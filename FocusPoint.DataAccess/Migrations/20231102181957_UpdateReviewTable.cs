using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FocusPointWeb.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReviewTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_MediaItems_MediaItemId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Packeges_PackegeId",
                table: "Reviews");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_MediaItemId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_PackegeId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "MediaItemId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "PackegeId",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "Reviews",
                newName: "EntityId");

            migrationBuilder.RenameColumn(
                name: "Comment",
                table: "Reviews",
                newName: "Text");

            migrationBuilder.AddColumn<string>(
                name: "EntityType",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_EntityId",
                table: "Reviews",
                column: "EntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_MediaItems_EntityId",
                table: "Reviews",
                column: "EntityId",
                principalTable: "MediaItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Packeges_EntityId",
                table: "Reviews",
                column: "EntityId",
                principalTable: "Packeges",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_MediaItems_EntityId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Packeges_EntityId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_EntityId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "EntityType",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "Text",
                table: "Reviews",
                newName: "Comment");

            migrationBuilder.RenameColumn(
                name: "EntityId",
                table: "Reviews",
                newName: "Rating");

            migrationBuilder.AddColumn<int>(
                name: "MediaItemId",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PackegeId",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MediaItemId = table.Column<int>(type: "int", nullable: false),
                    PackegeId = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfileUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SocialLink1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SocialLink2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SocialLink3 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfiles_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProfiles_MediaItems_MediaItemId",
                        column: x => x.MediaItemId,
                        principalTable: "MediaItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Packeges_PackegeId",
                        column: x => x.PackegeId,
                        principalTable: "Packeges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_MediaItemId",
                table: "Reviews",
                column: "MediaItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_PackegeId",
                table: "Reviews",
                column: "PackegeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_ApplicationUserId",
                table: "UserProfiles",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_MediaItemId",
                table: "UserProfiles",
                column: "MediaItemId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_PackegeId",
                table: "UserProfiles",
                column: "PackegeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_MediaItems_MediaItemId",
                table: "Reviews",
                column: "MediaItemId",
                principalTable: "MediaItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Packeges_PackegeId",
                table: "Reviews",
                column: "PackegeId",
                principalTable: "Packeges",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
