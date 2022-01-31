using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartParkingSystem.DataBase.Migrations
{
    public partial class AddMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "companyParkings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_companyParkings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "parkingSpaces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParkingNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParkingId = table.Column<int>(type: "int", nullable: false),
                    IsVacant = table.Column<bool>(type: "bit", nullable: false),
                    CarNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_parkingSpaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_parkingSpaces_companyParkings_ParkingId",
                        column: x => x.ParkingId,
                        principalTable: "companyParkings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_parkingSpaces_ParkingId",
                table: "parkingSpaces",
                column: "ParkingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "parkingSpaces");

            migrationBuilder.DropTable(
                name: "companyParkings");
        }
    }
}
