using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodBev.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddInterviewResponseAndPreferredProvince : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PreferredProvince",
                table: "JobPostings",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InterviewResponse",
                table: "Applications",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreferredProvince",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "InterviewResponse",
                table: "Applications");
        }
    }
}
