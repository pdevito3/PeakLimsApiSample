using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeakLims.Migrations
{
    public partial class TestOrderHAndlesTat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "due_date",
                table: "test_orders",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "tat_snapshot",
                table: "test_orders",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "due_date",
                table: "test_orders");

            migrationBuilder.DropColumn(
                name: "tat_snapshot",
                table: "test_orders");
        }
    }
}
