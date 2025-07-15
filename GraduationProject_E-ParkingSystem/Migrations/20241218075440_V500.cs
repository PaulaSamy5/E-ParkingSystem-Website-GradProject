using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject_E_ParkingSystem.Migrations
{
    /// <inheritdoc />
    public partial class V500 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFinished",
                table: "parkingRecords",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFinished",
                table: "parkingRecords");
        }
    }
}
