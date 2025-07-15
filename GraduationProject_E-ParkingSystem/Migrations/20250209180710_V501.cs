using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject_E_ParkingSystem.Migrations
{
    /// <inheritdoc />
    public partial class V501 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Users",
                newName: "FName");

            migrationBuilder.RenameColumn(
                name: "CarLicensePlate",
                table: "Users",
                newName: "LName");

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "LName",
                table: "Users",
                newName: "CarLicensePlate");

            migrationBuilder.RenameColumn(
                name: "FName",
                table: "Users",
                newName: "UserName");
        }
    }
}
