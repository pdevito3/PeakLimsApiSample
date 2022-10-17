using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeakLims.Migrations
{
    public partial class RequireContainerIdOnSample : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_samples_containers_container_id",
                table: "samples");

            migrationBuilder.AlterColumn<Guid>(
                name: "container_id",
                table: "samples",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_samples_containers_container_id",
                table: "samples",
                column: "container_id",
                principalTable: "containers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_samples_containers_container_id",
                table: "samples");

            migrationBuilder.AlterColumn<Guid>(
                name: "container_id",
                table: "samples",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "fk_samples_containers_container_id",
                table: "samples",
                column: "container_id",
                principalTable: "containers",
                principalColumn: "id");
        }
    }
}
