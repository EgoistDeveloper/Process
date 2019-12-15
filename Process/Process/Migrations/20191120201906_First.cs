using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Process.Migrations
{
    public partial class First : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
            name: "CalendarLogs",
            columns: table => new
            {
                Id = table.Column<long>(nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Date = table.Column<DateTime>(nullable: false),
                AddedDate = table.Column<DateTime>(nullable: false),
                Activity = table.Column<string>(nullable: false),
                IsDone = table.Column<bool>(nullable: false),
                Description = table.Column<string>(nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CalendarLogs", x => x.Id);
            });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
    name: "CalendarLogs");

        }
    }
}
