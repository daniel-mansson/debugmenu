using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DebugMenu.Silo.Migrations
{
    /// <inheritdoc />
    public partial class DebugMenuDbContext_Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userId = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    provider = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    providerAccountId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    refresh_token = table.Column<string>(type: "text", nullable: true),
                    access_token = table.Column<string>(type: "text", nullable: true),
                    expires_at = table.Column<long>(type: "bigint", nullable: true),
                    id_token = table.Column<string>(type: "text", nullable: true),
                    scope = table.Column<string>(type: "text", nullable: true),
                    session_state = table.Column<string>(type: "text", nullable: true),
                    token_type = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("accounts_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "applications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_applications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "sessions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userId = table.Column<int>(type: "integer", nullable: false),
                    expires = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    sessionToken = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("sessions_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    emailVerified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    image = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "verification_token",
                columns: table => new
                {
                    identifier = table.Column<string>(type: "text", nullable: false),
                    token = table.Column<string>(type: "text", nullable: false),
                    expires = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("verification_token_pkey", x => new { x.identifier, x.token });
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserEntity",
                columns: table => new
                {
                    UsersId = table.Column<int>(type: "integer", nullable: false),
                    ApplicationEntityId = table.Column<int>(type: "integer", nullable: false),
                    ApplicationsId = table.Column<int>(type: "integer", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserEntity", x => new { x.ApplicationEntityId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserEntity_applications_ApplicationsId",
                        column: x => x.ApplicationsId,
                        principalTable: "applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserEntity_users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserEntity_ApplicationsId",
                table: "ApplicationUserEntity",
                column: "ApplicationsId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserEntity_UsersId",
                table: "ApplicationUserEntity",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accounts");

            migrationBuilder.DropTable(
                name: "ApplicationUserEntity");

            migrationBuilder.DropTable(
                name: "sessions");

            migrationBuilder.DropTable(
                name: "verification_token");

            migrationBuilder.DropTable(
                name: "applications");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
