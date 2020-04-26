using Microsoft.EntityFrameworkCore.Migrations;

namespace Beeffective.Data.Migrations
{
    public partial class Add_Goal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Goal",
                table: "Tasks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Goal",
                table: "Tasks");
        }
    }
}
