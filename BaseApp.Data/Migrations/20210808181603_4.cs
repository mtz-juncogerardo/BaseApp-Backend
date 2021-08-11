using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseApp.Data.Migrations
{
    public partial class _4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuditDbModelId",
                table: "Article",
                type: "character varying(36)",
                maxLength: 36,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Article_AuditDbModelId",
                table: "Article",
                column: "AuditDbModelId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Article_Audit_AuditDbModelId",
                table: "Article",
                column: "AuditDbModelId",
                principalTable: "Audit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Article_Audit_AuditDbModelId",
                table: "Article");

            migrationBuilder.DropIndex(
                name: "IX_Article_AuditDbModelId",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "AuditDbModelId",
                table: "Article");
        }
    }
}
