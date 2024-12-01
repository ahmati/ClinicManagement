using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AspNetCoreHero.Boilerplate.Infrastructure.Migrations.ApplicationDb
{
    public partial class add_medical_data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MedicalDataId",
                table: "Patient",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MedicalData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HasIllness = table.Column<bool>(type: "bit", nullable: true),
                    IllnessDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsUnderTreatment = table.Column<bool>(type: "bit", nullable: true),
                    DoctorsName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicationsTreatment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SemundjeGjaku = table.Column<bool>(type: "bit", nullable: true),
                    TensionILarte = table.Column<bool>(type: "bit", nullable: true),
                    TensionIUlet = table.Column<bool>(type: "bit", nullable: true),
                    NderhyrjeKirugjikaleNeZemer = table.Column<bool>(type: "bit", nullable: true),
                    PropblemeZemre = table.Column<bool>(type: "bit", nullable: true),
                    EtheRaumatizmale = table.Column<bool>(type: "bit", nullable: true),
                    Glaucoma = table.Column<bool>(type: "bit", nullable: true),
                    AlergjiNgaLlastikuDorezave = table.Column<bool>(type: "bit", nullable: true),
                    AlergjiNgaMedikamentet = table.Column<bool>(type: "bit", nullable: true),
                    AlergjiNgaMetalet = table.Column<bool>(type: "bit", nullable: true),
                    SemundjeMendore = table.Column<bool>(type: "bit", nullable: true),
                    Epilepsi = table.Column<bool>(type: "bit", nullable: true),
                    PerdoruesDroge = table.Column<bool>(type: "bit", nullable: true),
                    AzemBronkiale = table.Column<bool>(type: "bit", nullable: true),
                    Turbekuloze = table.Column<bool>(type: "bit", nullable: true),
                    MjekimTumori = table.Column<bool>(type: "bit", nullable: true),
                    Shtatzen = table.Column<bool>(type: "bit", nullable: true),
                    AIDS = table.Column<bool>(type: "bit", nullable: true),
                    SemundjeMelcie = table.Column<bool>(type: "bit", nullable: true),
                    ArsyejaEParaqitjesNeKlinike = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalData", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Patient_MedicalDataId",
                table: "Patient",
                column: "MedicalDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patient_MedicalData_MedicalDataId",
                table: "Patient",
                column: "MedicalDataId",
                principalTable: "MedicalData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patient_MedicalData_MedicalDataId",
                table: "Patient");

            migrationBuilder.DropTable(
                name: "MedicalData");

            migrationBuilder.DropIndex(
                name: "IX_Patient_MedicalDataId",
                table: "Patient");

            migrationBuilder.DropColumn(
                name: "MedicalDataId",
                table: "Patient");
        }
    }
}
