using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeakLims.Migrations
{
    public partial class AddPatientInternalIdSequence : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "PAT",
                startValue: 10145702L);

            migrationBuilder.AlterColumn<string>(
                name: "internal_id",
                table: "patients",
                type: "text",
                nullable: false,
                defaultValueSql: "concat('PAT', nextval('\"PAT\"'))",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "PAT");

            migrationBuilder.AlterColumn<string>(
                name: "internal_id",
                table: "patients",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValueSql: "concat('PAT', nextval('\"PAT\"'))");
        }
    }
}
