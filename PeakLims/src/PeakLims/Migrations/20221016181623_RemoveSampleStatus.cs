using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeakLims.Migrations
{
    public partial class RemoveSampleStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "samples");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "samples",
                newName: "sex");

            migrationBuilder.CreateSequence(
                name: "SAM",
                startValue: 10000202L);

            migrationBuilder.AlterColumn<string>(
                name: "sample_number",
                table: "samples",
                type: "text",
                nullable: false,
                defaultValueSql: "concat('SAM', nextval('\"SAM\"'))",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "SAM");

            migrationBuilder.RenameColumn(
                name: "sex",
                table: "samples",
                newName: "type");

            migrationBuilder.AlterColumn<string>(
                name: "sample_number",
                table: "samples",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValueSql: "concat('SAM', nextval('\"SAM\"'))");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "samples",
                type: "text",
                nullable: true);
        }
    }
}
