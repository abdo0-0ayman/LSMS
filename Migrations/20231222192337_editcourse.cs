using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LSMS.Migrations
{
    /// <inheritdoc />
    public partial class editcourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "departmentId",
                table: "Courses",
                type: "nvarchar(450)",
                nullable: true,
                defaultValue: "");

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

            migrationBuilder.CreateIndex(
                name: "IX_Courses_departmentId",
                table: "Courses",
                column: "departmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Departments_departmentId",
                table: "Courses",
                column: "departmentId",
                principalTable: "Departments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Departments_departmentId",
                table: "Courses");

            migrationBuilder.DropTable(
                name: "Intersections");

            migrationBuilder.DropIndex(
                name: "IX_Courses_departmentId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "departmentId",
                table: "Courses");
        }
    }
}
