using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.API.Migrations
{
    /// <inheritdoc />
    public partial class ProfileInfoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProfileInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FacebookLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZaloLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TikTokLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileExtension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileSizeInBytes = table.Column<long>(type: "bigint", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileInfos", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7b72a55e-0189-4665-87ab-b8c4a44e00f0",
                columns: new[] { "ConcurrencyStamp", "CreateAt", "PasswordHash", "UpdateAt" },
                values: new object[] { "cf76663b-7a16-4003-b087-a6d822227538", new DateTime(2024, 11, 25, 19, 4, 27, 422, DateTimeKind.Utc).AddTicks(9894), "AQAAAAIAAYagAAAAENubiWM+nlLfe7w6DQ7ASjyhEbY1ZcYXb1XqayoFeFPeTEI7HiQb4gRL3uQ+JFkB2Q==", new DateTime(2024, 11, 25, 19, 4, 27, 422, DateTimeKind.Utc).AddTicks(9896) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfileInfos");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7b72a55e-0189-4665-87ab-b8c4a44e00f0",
                columns: new[] { "ConcurrencyStamp", "CreateAt", "PasswordHash", "UpdateAt" },
                values: new object[] { "ae37dc50-de91-42a8-b9ff-14191dc3ef6b", new DateTime(2024, 11, 25, 17, 3, 26, 685, DateTimeKind.Utc).AddTicks(2924), "AQAAAAIAAYagAAAAEL8L5tuQEdU+7ZvW7cO9Cr7AZAzoxllHJM84XANpPxVywDbXJ3C9lhdrjv7RXwTwNw==", new DateTime(2024, 11, 25, 17, 3, 26, 685, DateTimeKind.Utc).AddTicks(2926) });
        }
    }
}
