using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseApp.Data.Migrations
{
    public partial class _6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Article_Name",
                table: "Article",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Article_Name",
                table: "Article");
        }
    }
}
