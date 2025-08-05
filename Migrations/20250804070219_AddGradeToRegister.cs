using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ef2.Migrations
{
    public partial class AddGradeToRegister : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Grade",
                table: "Registers",
                type: "double precision",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Grade",
                table: "Registers");
        }
    }
}
