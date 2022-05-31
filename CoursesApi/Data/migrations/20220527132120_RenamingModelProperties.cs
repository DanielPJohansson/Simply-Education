using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoursesApi.Data.migrations
{
    public partial class RenamingModelProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Courses");

            migrationBuilder.AddColumn<double>(
                name: "DurationInHours",
                table: "Courses",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationInHours",
                table: "Courses");

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Courses",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
