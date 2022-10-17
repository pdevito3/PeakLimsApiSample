using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeakLims.Migrations
{
    public partial class TestOrderHasSample : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "sample_id",
                table: "test_orders",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_test_orders_sample_id",
                table: "test_orders",
                column: "sample_id");

            migrationBuilder.AddForeignKey(
                name: "fk_test_orders_samples_sample_id",
                table: "test_orders",
                column: "sample_id",
                principalTable: "samples",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_test_orders_samples_sample_id",
                table: "test_orders");

            migrationBuilder.DropIndex(
                name: "ix_test_orders_sample_id",
                table: "test_orders");

            migrationBuilder.DropColumn(
                name: "sample_id",
                table: "test_orders");
        }
    }
}
