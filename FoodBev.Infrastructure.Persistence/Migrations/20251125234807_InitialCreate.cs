using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodBev.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CandidateDetails",
                columns: table => new
                {
                    CandidateID = table.Column<int>(type: "INTEGER", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: true),
                    LastName = table.Column<string>(type: "TEXT", nullable: true),
                    IDNumber = table.Column<string>(type: "TEXT", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Race = table.Column<string>(type: "TEXT", nullable: true),
                    Gender = table.Column<string>(type: "TEXT", nullable: true),
                    IsDisabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    DisabilityDetails = table.Column<string>(type: "TEXT", nullable: true),
                    Nationality = table.Column<string>(type: "TEXT", nullable: true),
                    ContactNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    PhysicalAddress = table.Column<string>(type: "TEXT", nullable: true),
                    PostalCode = table.Column<string>(type: "TEXT", nullable: true),
                    Province = table.Column<string>(type: "TEXT", nullable: true),
                    HighestQualification = table.Column<string>(type: "TEXT", nullable: true),
                    InstitutionName = table.Column<string>(type: "TEXT", nullable: true),
                    QualificationYear = table.Column<int>(type: "INTEGER", nullable: true),
                    EmploymentStatus = table.Column<string>(type: "TEXT", nullable: true),
                    OFO_Code = table.Column<string>(type: "TEXT", nullable: true),
                    AcceptsPOPI = table.Column<bool>(type: "INTEGER", nullable: false),
                    ID_Document_Ref = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateDetails", x => x.CandidateID);
                });

            migrationBuilder.CreateTable(
                name: "EmployerDetails",
                columns: table => new
                {
                    EmployerID = table.Column<int>(type: "INTEGER", nullable: false),
                    UserID = table.Column<string>(type: "TEXT", nullable: true),
                    LNumber = table.Column<string>(type: "TEXT", nullable: true),
                    TNumber = table.Column<string>(type: "TEXT", nullable: true),
                    CompanyName = table.Column<string>(type: "TEXT", nullable: true),
                    LevyNumber = table.Column<string>(type: "TEXT", nullable: true),
                    SDFName = table.Column<string>(type: "TEXT", nullable: true),
                    SDFEmail = table.Column<string>(type: "TEXT", nullable: true),
                    SDFContactNumber = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployerDetails", x => x.EmployerID);
                });

            migrationBuilder.CreateTable(
                name: "OfoCodes",
                columns: table => new
                {
                    OfoCodeID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    Sector = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfoCodes", x => x.OfoCodeID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    UserType = table.Column<int>(type: "INTEGER", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "JobPostings",
                columns: table => new
                {
                    JobID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployerID = table.Column<int>(type: "INTEGER", nullable: false),
                    JobTitle = table.Column<string>(type: "TEXT", nullable: false),
                    JobDescription = table.Column<string>(type: "TEXT", nullable: false),
                    OFO_Code_Required = table.Column<string>(type: "TEXT", nullable: false),
                    IsBursary = table.Column<bool>(type: "INTEGER", nullable: false),
                    DatePosted = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ApplicationDeadline = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobPostings", x => x.JobID);
                    table.ForeignKey(
                        name: "FK_JobPostings_EmployerDetails_EmployerID",
                        column: x => x.EmployerID,
                        principalTable: "EmployerDetails",
                        principalColumn: "EmployerID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    ApplicationID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    JobID = table.Column<int>(type: "INTEGER", nullable: false),
                    CandidateID = table.Column<int>(type: "INTEGER", nullable: false),
                    DateApplied = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    InterviewDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    InterviewVenue = table.Column<string>(type: "TEXT", nullable: false),
                    CandidateAvailability = table.Column<string>(type: "TEXT", nullable: false),
                    CV_File_Ref = table.Column<string>(type: "TEXT", nullable: false),
                    AdminNotes = table.Column<string>(type: "TEXT", nullable: true),
                    RegistrationNumber = table.Column<string>(type: "TEXT", nullable: true),
                    DateOfRegistration = table.Column<DateTime>(type: "TEXT", nullable: true),
                    RegisteredBy = table.Column<string>(type: "TEXT", nullable: true),
                    EnrollmentFormDocumentRef = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.ApplicationID);
                    table.ForeignKey(
                        name: "FK_Applications_CandidateDetails_CandidateID",
                        column: x => x.CandidateID,
                        principalTable: "CandidateDetails",
                        principalColumn: "CandidateID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Applications_JobPostings_JobID",
                        column: x => x.JobID,
                        principalTable: "JobPostings",
                        principalColumn: "JobID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SkillsProgrammeForms",
                columns: table => new
                {
                    FormID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ApplicationID = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CandidateSignature = table.Column<string>(type: "TEXT", nullable: false),
                    EmployerSignature = table.Column<string>(type: "TEXT", nullable: false),
                    TrainingProviderSignature = table.Column<string>(type: "TEXT", nullable: false),
                    DateSubmitted = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateCompleted = table.Column<DateTime>(type: "TEXT", nullable: true),
                    AdminRegistrationNo = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillsProgrammeForms", x => x.FormID);
                    table.ForeignKey(
                        name: "FK_SkillsProgrammeForms_Applications_ApplicationID",
                        column: x => x.ApplicationID,
                        principalTable: "Applications",
                        principalColumn: "ApplicationID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Applications_CandidateID",
                table: "Applications",
                column: "CandidateID");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_JobID_CandidateID",
                table: "Applications",
                columns: new[] { "JobID", "CandidateID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CandidateDetails_IDNumber",
                table: "CandidateDetails",
                column: "IDNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployerDetails_LevyNumber",
                table: "EmployerDetails",
                column: "LevyNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobPostings_EmployerID",
                table: "JobPostings",
                column: "EmployerID");

            migrationBuilder.CreateIndex(
                name: "IX_SkillsProgrammeForms_ApplicationID",
                table: "SkillsProgrammeForms",
                column: "ApplicationID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfoCodes");

            migrationBuilder.DropTable(
                name: "SkillsProgrammeForms");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "CandidateDetails");

            migrationBuilder.DropTable(
                name: "JobPostings");

            migrationBuilder.DropTable(
                name: "EmployerDetails");
        }
    }
}
