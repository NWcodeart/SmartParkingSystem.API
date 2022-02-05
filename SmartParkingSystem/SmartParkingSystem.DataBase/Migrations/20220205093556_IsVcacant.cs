using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartParkingSystem.DataBase.Migrations
{
    public partial class IsVcacant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVacant",
                table: "parkingSpaces");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVacant",
                table: "parkingSpaces",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
