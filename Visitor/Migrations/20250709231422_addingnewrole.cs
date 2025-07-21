using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Visitor.Migrations
{
    /// <inheritdoc />
    public partial class addingnewrole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "C0969547-A084-4839-836C-F41F4CF5D123", null, "FloorSecurity", "FLOORSECURITY" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "DepartmentId",
                keyValue: "11111111-1111-1111-1111-111111111111",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8fa2f215-1f07-4c1e-b4e7-b0d9d3a137ab", "AQAAAAIAAYagAAAAEPs7AbE2DB06J2mhYnsssHSykqJX1q45Gmgpf4DijoczMpJb6Xhak0fUIuxupRsfYA==", "50f82e9c-6189-4858-9557-ac8713223b94" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "C0969547-A084-4839-836C-F41F4CF5D123");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "DepartmentId",
                keyValue: "11111111-1111-1111-1111-111111111111",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1a455a5c-88e4-4d4b-ad98-02f2270cd647", "AQAAAAIAAYagAAAAEGqErm0SjOxrFdmujRDKpx2YFiRbA0cLSpRBCpbFX83+qHxXTIdvVbTNfhW+cdLvCQ==", "f88e51d5-29e1-4e54-9213-05fefb1f7eb4" });
        }
    }
}
