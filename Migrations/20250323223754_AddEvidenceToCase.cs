using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CrimeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddEvidenceToCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignee_Cases_CaseId",
                table: "Assignee");

            migrationBuilder.DropForeignKey(
                name: "FK_Person_Cases_CaseId",
                table: "Person");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportedBy_Cases_CaseId",
                table: "ReportedBy");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReportedBy",
                table: "ReportedBy");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Person",
                table: "Person");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Assignee",
                table: "Assignee");

            migrationBuilder.RenameTable(
                name: "ReportedBy",
                newName: "CrimeReports");

            migrationBuilder.RenameTable(
                name: "Person",
                newName: "Persons");

            migrationBuilder.RenameTable(
                name: "Assignee",
                newName: "Assignees");

            migrationBuilder.RenameIndex(
                name: "IX_ReportedBy_CaseId",
                table: "CrimeReports",
                newName: "IX_CrimeReports_CaseId");

            migrationBuilder.RenameIndex(
                name: "IX_Person_CaseId",
                table: "Persons",
                newName: "IX_Persons_CaseId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignee_CaseId",
                table: "Assignees",
                newName: "IX_Assignees_CaseId");

            migrationBuilder.AlterColumn<int>(
                name: "CaseId",
                table: "CrimeReports",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CrimeReports",
                table: "CrimeReports",
                column: "ReportId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Persons",
                table: "Persons",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Assignees",
                table: "Assignees",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Evidences",
                columns: table => new
                {
                    EvidenceId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: false),
                    FilePath = table.Column<string>(type: "text", nullable: false),
                    CaseId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evidences", x => x.EvidenceId);
                    table.ForeignKey(
                        name: "FK_Evidences_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "CaseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Evidences_CaseId",
                table: "Evidences",
                column: "CaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignees_Cases_CaseId",
                table: "Assignees",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "CaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_CrimeReports_Cases_CaseId",
                table: "CrimeReports",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "CaseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_Cases_CaseId",
                table: "Persons",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "CaseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignees_Cases_CaseId",
                table: "Assignees");

            migrationBuilder.DropForeignKey(
                name: "FK_CrimeReports_Cases_CaseId",
                table: "CrimeReports");

            migrationBuilder.DropForeignKey(
                name: "FK_Persons_Cases_CaseId",
                table: "Persons");

            migrationBuilder.DropTable(
                name: "Evidences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Persons",
                table: "Persons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CrimeReports",
                table: "CrimeReports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Assignees",
                table: "Assignees");

            migrationBuilder.RenameTable(
                name: "Persons",
                newName: "Person");

            migrationBuilder.RenameTable(
                name: "CrimeReports",
                newName: "ReportedBy");

            migrationBuilder.RenameTable(
                name: "Assignees",
                newName: "Assignee");

            migrationBuilder.RenameIndex(
                name: "IX_Persons_CaseId",
                table: "Person",
                newName: "IX_Person_CaseId");

            migrationBuilder.RenameIndex(
                name: "IX_CrimeReports_CaseId",
                table: "ReportedBy",
                newName: "IX_ReportedBy_CaseId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignees_CaseId",
                table: "Assignee",
                newName: "IX_Assignee_CaseId");

            migrationBuilder.AlterColumn<int>(
                name: "CaseId",
                table: "ReportedBy",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Person",
                table: "Person",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReportedBy",
                table: "ReportedBy",
                column: "ReportId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Assignee",
                table: "Assignee",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignee_Cases_CaseId",
                table: "Assignee",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "CaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Person_Cases_CaseId",
                table: "Person",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "CaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedBy_Cases_CaseId",
                table: "ReportedBy",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "CaseId");
        }
    }
}
