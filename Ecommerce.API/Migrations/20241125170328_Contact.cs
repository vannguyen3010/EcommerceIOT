using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.API.Migrations
{
    /// <inheritdoc />
    public partial class Contact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7b72a55e-0189-4665-87ab-b8c4a44e00f0",
                columns: new[] { "ConcurrencyStamp", "CreateAt", "PasswordHash", "UpdateAt" },
                values: new object[] { "ae37dc50-de91-42a8-b9ff-14191dc3ef6b", new DateTime(2024, 11, 25, 17, 3, 26, 685, DateTimeKind.Utc).AddTicks(2924), "AQAAAAIAAYagAAAAEL8L5tuQEdU+7ZvW7cO9Cr7AZAzoxllHJM84XANpPxVywDbXJ3C9lhdrjv7RXwTwNw==", new DateTime(2024, 11, 25, 17, 3, 26, 685, DateTimeKind.Utc).AddTicks(2926) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7b72a55e-0189-4665-87ab-b8c4a44e00f0",
                columns: new[] { "ConcurrencyStamp", "CreateAt", "PasswordHash", "UpdateAt" },
                values: new object[] { "3faff1cb-8785-4d3d-bb62-a13275946960", new DateTime(2024, 11, 25, 8, 47, 6, 170, DateTimeKind.Utc).AddTicks(3388), "AQAAAAIAAYagAAAAEKWXmN1mQbEdYj6SaTr+eyziow5PD5ZW5ywznij5QLaYv9p/xJdwxsmsMnC2y9C34A==", new DateTime(2024, 11, 25, 8, 47, 6, 170, DateTimeKind.Utc).AddTicks(3390) });
        }
    }
}
