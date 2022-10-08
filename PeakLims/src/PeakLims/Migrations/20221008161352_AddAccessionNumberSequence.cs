using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeakLims.Migrations
{
    public partial class AddAccessionNumberSequence : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "ACC",
                startValue: 10005702L);

            migrationBuilder.AlterColumn<string>(
                name: "accession_number",
                table: "accessions",
                type: "text",
                nullable: false,
                defaultValueSql: "concat('ACC', nextval('\"ACC\"'))",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "ACC");

            migrationBuilder.AlterColumn<string>(
                name: "accession_number",
                table: "accessions",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValueSql: "concat('ACC', nextval('\"ACC\"'))");
        }
    }
}
