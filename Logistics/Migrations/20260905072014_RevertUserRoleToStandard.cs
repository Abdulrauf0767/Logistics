using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logistics.Migrations
{
    /// <inheritdoc />
    public partial class RevertUserRoleToStandard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersRole_Roles_RoleId",
                table: "UsersRole");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersRole_Users_UserId",
                table: "UsersRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersRole",
                table: "UsersRole");

            migrationBuilder.RenameTable(
                name: "UsersRole",
                newName: "UserRoles");

            migrationBuilder.RenameIndex(
                name: "IX_UsersRole_RoleId",
                table: "UserRoles",
                newName: "IX_UserRoles_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Roles_RoleId",
                table: "UserRoles",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_UserId",
                table: "UserRoles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Roles_RoleId",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_UserId",
                table: "UserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                newName: "UsersRole");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_RoleId",
                table: "UsersRole",
                newName: "IX_UsersRole_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersRole",
                table: "UsersRole",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersRole_Roles_RoleId",
                table: "UsersRole",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersRole_Users_UserId",
                table: "UsersRole",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
