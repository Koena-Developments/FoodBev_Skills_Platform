using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodBev.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTripartiteAgreement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TripartiteAgreements",
                columns: table => new
                {
                    AgreementID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ApplicationID = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CandidateSignature = table.Column<string>(type: "TEXT", nullable: true),
                    CandidateSignedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CandidateSignedByUserID = table.Column<int>(type: "INTEGER", nullable: true),
                    EmployerSignature = table.Column<string>(type: "TEXT", nullable: true),
                    EmployerSignedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    EmployerSignedByUserID = table.Column<int>(type: "INTEGER", nullable: true),
                    TrainingProviderSignatureFileRef = table.Column<string>(type: "TEXT", nullable: true),
                    TrainingProviderSignatureUploadDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TrainingProviderSignatureUploadedByUserID = table.Column<int>(type: "INTEGER", nullable: true),
                    SubmittedToAdminDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    AdminReviewedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    AdminReviewedByUserID = table.Column<int>(type: "INTEGER", nullable: true),
                    AdminNotes = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripartiteAgreements", x => x.AgreementID);
                    table.ForeignKey(
                        name: "FK_TripartiteAgreements_Applications_ApplicationID",
                        column: x => x.ApplicationID,
                        principalTable: "Applications",
                        principalColumn: "ApplicationID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TripartiteAgreements_ApplicationID",
                table: "TripartiteAgreements",
                column: "ApplicationID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TripartiteAgreements");
        }
    }
}
