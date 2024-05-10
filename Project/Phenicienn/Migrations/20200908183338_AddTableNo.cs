using Microsoft.EntityFrameworkCore.Migrations;

namespace Phenicienn.Migrations
{
    public partial class AddTableNo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TableNo",
                table: "tables",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tables_TableNo",
                table: "tables",
                column: "TableNo",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tables_TableNo",
                table: "tables");

            migrationBuilder.DropColumn(
                name: "TableNo",
                table: "tables");
        }
    }
}
