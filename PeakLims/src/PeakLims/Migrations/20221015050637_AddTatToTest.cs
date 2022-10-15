using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeakLims.Migrations
{
    public partial class AddTatToTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "turn_around_time",
                table: "tests",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "turn_around_time",
                table: "tests");
        }
    }
}
