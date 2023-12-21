using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DebugMenu.Silo.Migrations
{
    /// <inheritdoc />
    public partial class DebugMenuDbContext_Test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserEntity_applications_ApplicationsId",
                table: "ApplicationUserEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserEntity_users_UsersId",
                table: "ApplicationUserEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUserEntity",
                table: "ApplicationUserEntity");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUserEntity_UsersId",
                table: "ApplicationUserEntity");

            migrationBuilder.DropColumn(
                name: "ApplicationEntityId",
                table: "ApplicationUserEntity");

            migrationBuilder.RenameTable(
                name: "ApplicationUserEntity",
                newName: "applications_users");

            migrationBuilder.RenameColumn(
                name: "ApplicationsId",
                table: "applications_users",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "applications_users",
                newName: "ApplicationId");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUserEntity_ApplicationsId",
                table: "applications_users",
                newName: "IX_applications_users_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_applications_users",
                table: "applications_users",
                columns: new[] { "ApplicationId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_applications_users_applications_ApplicationId",
                table: "applications_users",
                column: "ApplicationId",
                principalTable: "applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_applications_users_users_UserId",
                table: "applications_users",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_applications_users_applications_ApplicationId",
                table: "applications_users");

            migrationBuilder.DropForeignKey(
                name: "FK_applications_users_users_UserId",
                table: "applications_users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_applications_users",
                table: "applications_users");

            migrationBuilder.RenameTable(
                name: "applications_users",
                newName: "ApplicationUserEntity");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ApplicationUserEntity",
                newName: "ApplicationsId");

            migrationBuilder.RenameColumn(
                name: "ApplicationId",
                table: "ApplicationUserEntity",
                newName: "UsersId");

            migrationBuilder.RenameIndex(
                name: "IX_applications_users_UserId",
                table: "ApplicationUserEntity",
                newName: "IX_ApplicationUserEntity_ApplicationsId");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationEntityId",
                table: "ApplicationUserEntity",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUserEntity",
                table: "ApplicationUserEntity",
                columns: new[] { "ApplicationEntityId", "UsersId" });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserEntity_UsersId",
                table: "ApplicationUserEntity",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserEntity_applications_ApplicationsId",
                table: "ApplicationUserEntity",
                column: "ApplicationsId",
                principalTable: "applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserEntity_users_UsersId",
                table: "ApplicationUserEntity",
                column: "UsersId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
