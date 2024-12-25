using Microsoft.EntityFrameworkCore.Migrations;

namespace AspNetCoreHero.Boilerplate.Infrastructure.Migrations.ApplicationDb
{
    public partial class fixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PropblemeZemre",
                table: "MedicalData",
                newName: "ProblemeZemre");

            migrationBuilder.AddColumn<bool>(
                name: "Diabet",
                table: "MedicalData",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Diabet",
                table: "MedicalData");

            migrationBuilder.RenameColumn(
                name: "ProblemeZemre",
                table: "MedicalData",
                newName: "PropblemeZemre");
        }
    }
}
