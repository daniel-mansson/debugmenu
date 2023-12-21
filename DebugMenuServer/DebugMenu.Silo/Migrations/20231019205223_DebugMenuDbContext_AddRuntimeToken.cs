using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DebugMenu.Silo.Migrations
{
    /// <inheritdoc />
    public partial class DebugMenuDbContext_AddRuntimeToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "runtime_tokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    ApplicationId = table.Column<int>(type: "integer", nullable: false),
                    ApplicationEntityId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_runtime_tokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_runtime_tokens_applications_ApplicationEntityId",
                        column: x => x.ApplicationEntityId,
                        principalTable: "applications",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_runtime_tokens_applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_runtime_tokens_ApplicationEntityId",
                table: "runtime_tokens",
                column: "ApplicationEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_runtime_tokens_ApplicationId",
                table: "runtime_tokens",
                column: "ApplicationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "runtime_tokens");
        }
    }
}
