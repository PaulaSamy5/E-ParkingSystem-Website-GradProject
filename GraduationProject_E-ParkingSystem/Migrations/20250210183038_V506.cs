using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject_E_ParkingSystem.Migrations
{
    /// <inheritdoc />
    public partial class V506 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                table: "parkingRecords",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalCostPaid = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_parkingRecords_PaymentId",
                table: "parkingRecords",
                column: "PaymentId",
                unique: true,
                filter: "[PaymentId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_parkingRecords_payments_PaymentId",
                table: "parkingRecords",
                column: "PaymentId",
                principalTable: "payments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_parkingRecords_payments_PaymentId",
                table: "parkingRecords");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropIndex(
                name: "IX_parkingRecords_PaymentId",
                table: "parkingRecords");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "parkingRecords");
        }
    }
}
