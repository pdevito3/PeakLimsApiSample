using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeakLims.Migrations
{
    public partial class SimplifyAccessionCommentProps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ending_accession_state",
                table: "accession_comments");

            migrationBuilder.DropColumn(
                name: "original_comment_id",
                table: "accession_comments");

            migrationBuilder.RenameColumn(
                name: "initial_accession_state",
                table: "accession_comments",
                newName: "status");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "status",
                table: "accession_comments",
                newName: "initial_accession_state");

            migrationBuilder.AddColumn<string>(
                name: "ending_accession_state",
                table: "accession_comments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "original_comment_id",
                table: "accession_comments",
                type: "uuid",
                nullable: true);
        }
    }
}
