using Microsoft.EntityFrameworkCore.Migrations;

namespace AspNetCoreHero.Boilerplate.Infrastructure.Migrations.ApplicationDb
{
    public partial class fix_patient_treatment2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientTreatment_StaffUser_StaffUserId",
                table: "PatientTreatment");

            migrationBuilder.AlterColumn<int>(
                name: "StaffUserId",
                table: "PatientTreatment",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientTreatment_StaffUser_StaffUserId",
                table: "PatientTreatment",
                column: "StaffUserId",
                principalTable: "StaffUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientTreatment_StaffUser_StaffUserId",
                table: "PatientTreatment");

            migrationBuilder.AlterColumn<int>(
                name: "StaffUserId",
                table: "PatientTreatment",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientTreatment_StaffUser_StaffUserId",
                table: "PatientTreatment",
                column: "StaffUserId",
                principalTable: "StaffUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
