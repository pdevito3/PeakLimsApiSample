using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeakLims.Migrations
{
    public partial class ChangeStatePropsToStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "state",
                table: "test_orders",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "state",
                table: "samples",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "state",
                table: "panel_orders",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "state",
                table: "containers",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "state",
                table: "accessions",
                newName: "status");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "status",
                table: "test_orders",
                newName: "state");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "samples",
                newName: "state");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "panel_orders",
                newName: "state");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "containers",
                newName: "state");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "accessions",
                newName: "state");
        }
    }
}
