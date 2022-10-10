using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeakLims.Migrations
{
    public partial class AddTestStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_accessions_healthcare_organizations_healthcare_organization",
                table: "accessions");

            migrationBuilder.DropForeignKey(
                name: "fk_accessions_patients_patient_id",
                table: "accessions");

            migrationBuilder.DropIndex(
                name: "ix_accessions_healthcare_organization_id",
                table: "accessions");

            migrationBuilder.DropIndex(
                name: "ix_accessions_patient_id",
                table: "accessions");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "tests",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "tests");

            migrationBuilder.CreateIndex(
                name: "ix_accessions_healthcare_organization_id",
                table: "accessions",
                column: "healthcare_organization_id");

            migrationBuilder.CreateIndex(
                name: "ix_accessions_patient_id",
                table: "accessions",
                column: "patient_id");

            migrationBuilder.AddForeignKey(
                name: "fk_accessions_healthcare_organizations_healthcare_organization",
                table: "accessions",
                column: "healthcare_organization_id",
                principalTable: "healthcare_organizations",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_accessions_patients_patient_id",
                table: "accessions",
                column: "patient_id",
                principalTable: "patients",
                principalColumn: "id");
        }
    }
}
