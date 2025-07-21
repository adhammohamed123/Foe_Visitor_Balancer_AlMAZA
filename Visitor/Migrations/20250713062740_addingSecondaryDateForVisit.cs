using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Visitor.Migrations
{
    /// <inheritdoc />
    public partial class addingSecondaryDateForVisit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPraimaryDateAccepted",
                table: "Visits",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PrimaryDate",
                table: "Visits",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "SecondaryDate",
                table: "Visits",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "DepartmentId",
                keyValue: "11111111-1111-1111-1111-111111111111",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ee71a4ff-7289-4e36-9751-421c9435087e", "AQAAAAIAAYagAAAAEMTp4vpAj4BWhZTCwUQb4/78cr/t55Xy1AFYd0oZzQ9u1HQTulDbvuE51/sU2plVfA==", "729d27e1-045e-4e9d-9b9f-dd081fb21532" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPraimaryDateAccepted",
                table: "Visits");

            migrationBuilder.DropColumn(
                name: "PrimaryDate",
                table: "Visits");

            migrationBuilder.DropColumn(
                name: "SecondaryDate",
                table: "Visits");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "DepartmentId",
                keyValue: "11111111-1111-1111-1111-111111111111",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8fa2f215-1f07-4c1e-b4e7-b0d9d3a137ab", "AQAAAAIAAYagAAAAEPs7AbE2DB06J2mhYnsssHSykqJX1q45Gmgpf4DijoczMpJb6Xhak0fUIuxupRsfYA==", "50f82e9c-6189-4858-9557-ac8713223b94" });
        }
    }
}
