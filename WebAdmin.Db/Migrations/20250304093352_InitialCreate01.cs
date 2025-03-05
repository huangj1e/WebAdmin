using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAdmin.Migrations
{
    public partial class InitialCreate01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "状态",
                table: "SiteModels");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "状态",
                table: "SiteModels",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
