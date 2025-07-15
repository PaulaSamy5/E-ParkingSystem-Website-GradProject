using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject_E_ParkingSystem.Migrations
{
    /// <inheritdoc />
    public partial class V502 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
    name: "ParkingSpots",
    newName: "ParkingSlots"
);

            migrationBuilder.RenameColumn(
                name: "SpotNumber",
                table: "ParkingSpots",
                newName: "SlotName");

            migrationBuilder.AlterColumn<string>(
                name: "LName",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
    name: "ParkingSlots",
    newName: "ParkingSpots"
);

            migrationBuilder.RenameColumn(
                name: "SlotName",
                table: "ParkingSpots",
                newName: "SpotNumber");

            migrationBuilder.AlterColumn<string>(
                name: "LName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }
    }
}
