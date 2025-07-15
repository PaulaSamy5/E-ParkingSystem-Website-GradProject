using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject_E_ParkingSystem.Migrations
{
    /// <inheritdoc />
    public partial class VVV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "payments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParkingRecordid = table.Column<int>(type: "int", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_payments_parkingRecords_ParkingRecordid",
                        column: x => x.ParkingRecordid,
                        principalTable: "parkingRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_payments_ParkingRecordid",
                table: "payments",
                column: "ParkingRecordid",
                unique: true);
        }
    }
}
