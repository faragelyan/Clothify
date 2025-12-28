using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clothify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyUserAttributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ProfileImageUrl",
                table: "AspNetUsers",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "AspNetUsers",
                type: "varchar(500)",
                unicode: false,
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "DateOfBirth", "PasswordHash", "ProfileImageUrl", "RefreshToken", "RefreshTokenExpiryTime", "SecurityStamp" },
                values: new object[] { "b7785c72-ab51-4b29-8c7f-e7536af0054a", new DateTime(2025, 12, 28, 17, 25, 23, 876, DateTimeKind.Utc).AddTicks(5022), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AQAAAAIAAYagAAAAELrA2Ns7et/0IrtvxXq1kq3Y7APsACNS3DVl8dcCbCfBm1I9GLp8oqDdnEi8ZBCJ3Q==", null, null, null, "1bd3f485-00c0-4f29-8b41-14090debbf1c" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfileImageUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "999162d5-c03e-4d5c-a013-e6c586635bce", new DateTime(2025, 12, 28, 15, 10, 54, 538, DateTimeKind.Utc).AddTicks(704), "AQAAAAIAAYagAAAAEAHeZRZLEiRY7arczl/U+Q2l/lNelXn5UoH1OaXefY+zckcDqTOMiCc3t2sbAcea/w==", "67278000-3bfb-48cd-afe6-35c643d5514c" });
        }
    }
}
