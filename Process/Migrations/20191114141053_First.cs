using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Process.Migrations
{
    public partial class First : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiaryBlackWords",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Word = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiaryBlackWords", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiaryBlackWords_Word",
                table: "DiaryBlackWords",
                column: "Word",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
            name: "DiaryBlackWords");
        }
    }
}
