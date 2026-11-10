using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FocusPointWeb.Migrations
{
    /// <inheritdoc />
    public partial class ModifyBookingHeder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderTotal",
                table: "BookingHeders",
                newName: "BookingTotal");

            migrationBuilder.RenameColumn(
                name: "OrderStatus",
                table: "BookingHeders",
                newName: "BookingStatus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BookingTotal",
                table: "BookingHeders",
                newName: "OrderTotal");

            migrationBuilder.RenameColumn(
                name: "BookingStatus",
                table: "BookingHeders",
                newName: "OrderStatus");
        }
    }
}
