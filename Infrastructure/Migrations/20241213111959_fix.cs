using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamMembers",
                table: "TeamMembers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamMember",
                table: "TeamMembers",
                columns: new[] { "ProjectId", "UserId", "RoleId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamMember",
                table: "TeamMembers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamMembers",
                table: "TeamMembers",
                columns: new[] { "ProjectId", "UserId", "RoleId" });
        }
    }
}
