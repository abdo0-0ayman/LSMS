using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LSMS.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Halls",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    capacity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Halls", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Intersections",
                columns: table => new
                {
                    departmentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    lectureId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Intersections", x => new { x.departmentId, x.lectureId });
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Salt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    hours = table.Column<int>(type: "int", nullable: false),
                    departmentId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.id);
                    table.ForeignKey(
                        name: "FK_Courses_Departments_departmentId",
                        column: x => x.departmentId,
                        principalTable: "Departments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Professors",
                columns: table => new
                {
                    SSN = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    departmentId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professors", x => x.SSN);
                    table.ForeignKey(
                        name: "FK_Professors_Departments_departmentId",
                        column: x => x.departmentId,
                        principalTable: "Departments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    SSN = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    academicEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    departmentId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.SSN);
                    table.ForeignKey(
                        name: "FK_Students_Departments_departmentId",
                        column: x => x.departmentId,
                        principalTable: "Departments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lectures",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    courseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    professorSSN = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    hallId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    lectureNum = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lectures", x => x.id);
                    table.ForeignKey(
                        name: "FK_Lectures_Courses_courseId",
                        column: x => x.courseId,
                        principalTable: "Courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lectures_Halls_hallId",
                        column: x => x.hallId,
                        principalTable: "Halls",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Lectures_Professors_professorSSN",
                        column: x => x.professorSSN,
                        principalTable: "Professors",
                        principalColumn: "SSN",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Enrollments",
                columns: table => new
                {
                    studentSSN = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    lectureId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollments", x => new { x.studentSSN, x.lectureId });
                    table.ForeignKey(
                        name: "FK_Enrollments_Lectures_lectureId",
                        column: x => x.lectureId,
                        principalTable: "Lectures",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Enrollments_Students_studentSSN",
                        column: x => x.studentSSN,
                        principalTable: "Students",
                        principalColumn: "SSN",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_departmentId",
                table: "Courses",
                column: "departmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_lectureId",
                table: "Enrollments",
                column: "lectureId");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_courseId",
                table: "Lectures",
                column: "courseId");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_hallId",
                table: "Lectures",
                column: "hallId");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_professorSSN",
                table: "Lectures",
                column: "professorSSN");

            migrationBuilder.CreateIndex(
                name: "IX_Professors_departmentId",
                table: "Professors",
                column: "departmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_departmentId",
                table: "Students",
                column: "departmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Enrollments");

            migrationBuilder.DropTable(
                name: "Intersections");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Lectures");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Halls");

            migrationBuilder.DropTable(
                name: "Professors");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
