using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LSMS.Migrations
{
    /// <inheritdoc />
    public partial class i : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseProfessors_Halls_HallId",
                table: "CourseProfessors");

            migrationBuilder.DropIndex(
                name: "IX_CourseProfessors_HallId",
                table: "CourseProfessors");

            migrationBuilder.DropColumn(
                name: "HallId",
                table: "CourseProfessors");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HallId",
                table: "CourseProfessors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CourseProfessors_HallId",
                table: "CourseProfessors",
                column: "HallId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseProfessors_Halls_HallId",
                table: "CourseProfessors",
                column: "HallId",
                principalTable: "Halls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
