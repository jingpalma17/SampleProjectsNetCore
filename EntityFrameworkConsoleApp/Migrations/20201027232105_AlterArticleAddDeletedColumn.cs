using Microsoft.EntityFrameworkCore.Migrations;

namespace SampleProjectsNetCore.Migrations
{
    public partial class AlterArticleAddDeletedColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Deleted",
                table: "Articles",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Articles");
        }
    }
}
