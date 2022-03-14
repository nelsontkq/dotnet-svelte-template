using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotnetSvelteAuthApp.Migrations
{
    public partial class AddUserRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "147bbf19-cbbe-4c4a-a262-09b19d427a49", "b37e42b3-1c01-4b80-8316-dad8940c5e7e", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "24bebbc3-264c-4735-84a0-b17ac06a80f7", "9ee9ff35-ff51-4cdb-baaa-8f3b26ab786b", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "147bbf19-cbbe-4c4a-a262-09b19d427a49");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "24bebbc3-264c-4735-84a0-b17ac06a80f7");
        }
    }
}
