using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject_E_ParkingSystem.Migrations
{
    /// <inheritdoc />
    public partial class v22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkingRecord_ParkingSpots_ParkingSpotId",
                table: "ParkingRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkingRecord_Users_UserId",
                table: "ParkingRecord");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParkingSetting",
                table: "ParkingSetting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParkingRecord",
                table: "ParkingRecord");

            migrationBuilder.RenameTable(
                name: "ParkingSetting",
                newName: "parkingSettings");

            migrationBuilder.RenameTable(
                name: "ParkingRecord",
                newName: "parkingRecords");

            migrationBuilder.RenameIndex(
                name: "IX_ParkingRecord_UserId",
                table: "parkingRecords",
                newName: "IX_parkingRecords_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ParkingRecord_ParkingSpotId",
                table: "parkingRecords",
                newName: "IX_parkingRecords_ParkingSpotId");

            migrationBuilder.AddColumn<string>(
                name: "PlateNumber",
                table: "parkingRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "vehicleType",
                table: "parkingRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_parkingSettings",
                table: "parkingSettings",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_parkingRecords",
                table: "parkingRecords",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_parkingRecords_ParkingSpots_ParkingSpotId",
                table: "parkingRecords",
                column: "ParkingSpotId",
                principalTable: "ParkingSpots",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_parkingRecords_Users_UserId",
                table: "parkingRecords",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_parkingRecords_ParkingSpots_ParkingSpotId",
                table: "parkingRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_parkingRecords_Users_UserId",
                table: "parkingRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_parkingSettings",
                table: "parkingSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_parkingRecords",
                table: "parkingRecords");

            migrationBuilder.DropColumn(
                name: "PlateNumber",
                table: "parkingRecords");

            migrationBuilder.DropColumn(
                name: "vehicleType",
                table: "parkingRecords");

            migrationBuilder.RenameTable(
                name: "parkingSettings",
                newName: "ParkingSetting");

            migrationBuilder.RenameTable(
                name: "parkingRecords",
                newName: "ParkingRecord");

            migrationBuilder.RenameIndex(
                name: "IX_parkingRecords_UserId",
                table: "ParkingRecord",
                newName: "IX_ParkingRecord_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_parkingRecords_ParkingSpotId",
                table: "ParkingRecord",
                newName: "IX_ParkingRecord_ParkingSpotId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParkingSetting",
                table: "ParkingSetting",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParkingRecord",
                table: "ParkingRecord",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingRecord_ParkingSpots_ParkingSpotId",
                table: "ParkingRecord",
                column: "ParkingSpotId",
                principalTable: "ParkingSpots",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingRecord_Users_UserId",
                table: "ParkingRecord",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
