using Microsoft.EntityFrameworkCore.Migrations;

namespace AspNetCoreHero.Boilerplate.Infrastructure.Migrations.ApplicationDb
{
    public partial class fix_patient_treatment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StaffUserId",
                table: "PatientTreatment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PatientTreatment_StaffUserId",
                table: "PatientTreatment",
                column: "StaffUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientTreatment_StaffUser_StaffUserId",
                table: "PatientTreatment",
                column: "StaffUserId",
                principalTable: "StaffUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientTreatment_StaffUser_StaffUserId",
                table: "PatientTreatment");

            migrationBuilder.DropIndex(
                name: "IX_PatientTreatment_StaffUserId",
                table: "PatientTreatment");

            migrationBuilder.DropColumn(
                name: "StaffUserId",
                table: "PatientTreatment");
        }
    }
}
