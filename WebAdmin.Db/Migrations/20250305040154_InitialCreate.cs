using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAdmin.Db.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SiteModels",
                columns: table => new
                {
                    任务Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    网址 = table.Column<string>(type: "TEXT", nullable: false),
                    姓名 = table.Column<string>(type: "TEXT", nullable: false),
                    描述 = table.Column<string>(type: "TEXT", nullable: false),
                    最后扫描时间 = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteModels", x => x.任务Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SiteModels");
        }
    }
}
