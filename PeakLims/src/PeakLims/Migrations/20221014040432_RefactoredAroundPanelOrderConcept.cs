using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeakLims.Migrations
{
    public partial class RefactoredAroundPanelOrderConcept : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "panel_orders");

            migrationBuilder.AddColumn<Guid>(
                name: "associated_panel_id",
                table: "test_orders",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_test_orders_associated_panel_id",
                table: "test_orders",
                column: "associated_panel_id");

            migrationBuilder.AddForeignKey(
                name: "fk_test_orders_panels_associated_panel_id",
                table: "test_orders",
                column: "associated_panel_id",
                principalTable: "panels",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_test_orders_panels_associated_panel_id",
                table: "test_orders");

            migrationBuilder.DropIndex(
                name: "ix_test_orders_associated_panel_id",
                table: "test_orders");

            migrationBuilder.DropColumn(
                name: "associated_panel_id",
                table: "test_orders");

            migrationBuilder.CreateTable(
                name: "panel_orders",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    panel_id = table.Column<Guid>(type: "uuid", nullable: true),
                    accession_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    last_modified_by = table.Column<string>(type: "text", nullable: true),
                    last_modified_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_panel_orders", x => x.id);
                    table.ForeignKey(
                        name: "fk_panel_orders_accessions_accession_id",
                        column: x => x.accession_id,
                        principalTable: "accessions",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_panel_orders_panels_panel_id",
                        column: x => x.panel_id,
                        principalTable: "panels",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_panel_orders_accession_id",
                table: "panel_orders",
                column: "accession_id");

            migrationBuilder.CreateIndex(
                name: "ix_panel_orders_panel_id",
                table: "panel_orders",
                column: "panel_id");
        }
    }
}
