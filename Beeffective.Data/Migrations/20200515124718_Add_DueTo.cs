using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Beeffective.Data.Migrations
{
    public partial class Add_DueTo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DueTo",
                table: "Tasks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DueTo",
                table: "Tasks");
        }
    }
}
