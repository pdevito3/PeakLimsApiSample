using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeakLims.Migrations
{
    public partial class AddAddressToHealthcareOrg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "primary_address_city",
                table: "healthcare_organizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "primary_address_country",
                table: "healthcare_organizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "primary_address_line1",
                table: "healthcare_organizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "primary_address_line2",
                table: "healthcare_organizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "primary_address_postal_code",
                table: "healthcare_organizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "primary_address_state",
                table: "healthcare_organizations",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "primary_address_city",
                table: "healthcare_organizations");

            migrationBuilder.DropColumn(
                name: "primary_address_country",
                table: "healthcare_organizations");

            migrationBuilder.DropColumn(
                name: "primary_address_line1",
                table: "healthcare_organizations");

            migrationBuilder.DropColumn(
                name: "primary_address_line2",
                table: "healthcare_organizations");

            migrationBuilder.DropColumn(
                name: "primary_address_postal_code",
                table: "healthcare_organizations");

            migrationBuilder.DropColumn(
                name: "primary_address_state",
                table: "healthcare_organizations");
        }
    }
}
