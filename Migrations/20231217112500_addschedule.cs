using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LSMS.Migrations
{
    /// <inheritdoc />
    public partial class addschedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_CourseProfessors_CourseProfessorId",
                table: "Enrollments");

            migrationBuilder.DropTable(
                name: "CourseProfessors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Enrollments",
                table: "Enrollments");

            migrationBuilder.DropIndex(
                name: "IX_Enrollments_CourseProfessorId",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "CourseProfessorId",
                table: "Enrollments");

            migrationBuilder.AddColumn<string>(
                name: "LectureId",
                table: "Enrollments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Enrollments",
                table: "Enrollments",
                columns: new[] { "StudentId", "LectureId" });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lectures",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProfessorId = table.Column<int>(type: "int", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ScheduleId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lectures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lectures_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lectures_Professors_ProfessorId",
                        column: x => x.ProfessorId,
                        principalTable: "Professors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lectures_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_LectureId",
                table: "Enrollments",
                column: "LectureId");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_CourseId",
                table: "Lectures",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_ProfessorId",
                table: "Lectures",
                column: "ProfessorId");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_ScheduleId",
                table: "Lectures",
                column: "ScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Lectures_LectureId",
                table: "Enrollments",
                column: "LectureId",
                principalTable: "Lectures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Lectures_LectureId",
                table: "Enrollments");

            migrationBuilder.DropTable(
                name: "Lectures");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Enrollments",
                table: "Enrollments");

            migrationBuilder.DropIndex(
                name: "IX_Enrollments_LectureId",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "LectureId",
                table: "Enrollments");

            migrationBuilder.AddColumn<int>(
                name: "CourseProfessorId",
                table: "Enrollments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Enrollments",
                table: "Enrollments",
                columns: new[] { "StudentId", "CourseProfessorId" });

            migrationBuilder.CreateTable(
                name: "CourseProfessors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HallId = table.Column<int>(type: "int", nullable: false),
                    ProfessorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseProfessors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseProfessors_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseProfessors_Halls_HallId",
                        column: x => x.HallId,
                        principalTable: "Halls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseProfessors_Professors_ProfessorId",
                        column: x => x.ProfessorId,
                        principalTable: "Professors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_CourseProfessorId",
                table: "Enrollments",
                column: "CourseProfessorId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseProfessors_CourseId",
                table: "CourseProfessors",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseProfessors_HallId",
                table: "CourseProfessors",
                column: "HallId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseProfessors_ProfessorId",
                table: "CourseProfessors",
                column: "ProfessorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_CourseProfessors_CourseProfessorId",
                table: "Enrollments",
                column: "CourseProfessorId",
                principalTable: "CourseProfessors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
