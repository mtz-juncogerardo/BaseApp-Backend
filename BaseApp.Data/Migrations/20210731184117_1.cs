using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseApp.Data.Migrations
{
    public partial class _1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Audit",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    CreatedAtDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedByUserEmail = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UpdatedByUserEmail = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    LastTouchedByIp = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    UpdatedAtDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Authentication",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    Salt = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    EmailValid = table.Column<bool>(type: "boolean", nullable: false),
                    VersionId = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authentication", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    AuthenticationDbModelId = table.Column<string>(type: "character varying(36)", nullable: false),
                    AuditDbModelId = table.Column<string>(type: "character varying(36)", nullable: false),
                    Name = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true),
                    Phone = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: true),
                    Address = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    City = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    State = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    PostalCode = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Audit_AuditDbModelId",
                        column: x => x.AuditDbModelId,
                        principalTable: "Audit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Authentication_AuthenticationDbModelId",
                        column: x => x.AuthenticationDbModelId,
                        principalTable: "Authentication",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_AuditDbModelId",
                table: "Users",
                column: "AuditDbModelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_AuthenticationDbModelId",
                table: "Users",
                column: "AuthenticationDbModelId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Audit");

            migrationBuilder.DropTable(
                name: "Authentication");
        }
    }
}
