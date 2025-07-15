using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject_E_ParkingSystem.Migrations
{
    /// <inheritdoc />
    public partial class V6 : Migration
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

            migrationBuilder.DropColumn(
                name: "PricePerHour",
                table: "ParkingRecord");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "ParkingRecord",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ParkingSpotId",
                table: "ParkingRecord",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "ParkingRecord",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkingRecord_ParkingSpots_ParkingSpotId",
                table: "ParkingRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkingRecord_Users_UserId",
                table: "ParkingRecord");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "ParkingRecord");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "ParkingRecord",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ParkingSpotId",
                table: "ParkingRecord",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PricePerHour",
                table: "ParkingRecord",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingRecord_ParkingSpots_ParkingSpotId",
                table: "ParkingRecord",
                column: "ParkingSpotId",
                principalTable: "ParkingSpots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingRecord_Users_UserId",
                table: "ParkingRecord",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
