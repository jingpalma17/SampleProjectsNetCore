using Microsoft.EntityFrameworkCore.Migrations;

namespace SampleProjectsNetCore.Migrations
{
    public partial class AlterArticleChangeDeletedColumnType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                        name: "DeletedTemp",
                        table: "Articles",
                        nullable: false,
                        defaultValue: false);

            migrationBuilder.Sql(
                "UPDATE Articles " +
                "SET DeletedTemp = " +
                    "CASE Deleted " +
                        "WHEN 0 THEN 'False' " +
                        "WHEN 1 THEN 'True' " +
                        "ELSE 'False' " +
                    "END");

            migrationBuilder.DropColumn(
               name: "Deleted",
               table: "Articles");

            migrationBuilder.RenameColumn(
               name: "DeletedTemp",
               table: "Articles",
               newName: "Deleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
            name: "DeletedTemp",
            table: "Articles",
            nullable: false,
            defaultValue: 1);

            migrationBuilder.Sql(
                "UPDATE Articles " +
                "SET DeletedTemp = " +
                    "CASE Deleted " +
                        "WHEN 'False' THEN 0 " +
                        "WHEN 'True' THEN 1 " +
                        "ELSE 0 " +
                    "END ");

            migrationBuilder.DropColumn(
               name: "Deleted",
               table: "Articles");

            migrationBuilder.RenameColumn(
               name: "DeletedTemp",
               table: "Articles",
               newName: "Deleted");
        }
    }
}
