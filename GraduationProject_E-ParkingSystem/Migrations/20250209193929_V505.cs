using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject_E_ParkingSystem.Migrations
{
    /// <inheritdoc />
    public partial class V505 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
    name: "Slot",
    newName: "ParkingSpots"
);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
    name: "ParkingSpots",
    newName: "Slot"
);

        }
    }
}
