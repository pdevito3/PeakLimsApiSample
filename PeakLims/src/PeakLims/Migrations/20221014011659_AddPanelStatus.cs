using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeakLims.Migrations
{
    public partial class AddPanelStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "panels",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "panels");
        }
    }
}
