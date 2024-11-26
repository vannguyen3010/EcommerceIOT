using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.API.Migrations
{
    /// <inheritdoc />
    public partial class CateproductProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CateProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameSlug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CateProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CateProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CateProducts_CateProducts_CateProductId",
                        column: x => x.CateProductId,
                        principalTable: "CateProducts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameSlug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Detail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageFileExtension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageFileSizeInBytes = table.Column<long>(type: "bigint", nullable: true),
                    ImageFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsHot = table.Column<bool>(type: "bit", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_CateProducts_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "CateProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7b72a55e-0189-4665-87ab-b8c4a44e00f0",
                columns: new[] { "ConcurrencyStamp", "CreateAt", "PasswordHash", "UpdateAt" },
                values: new object[] { "e7e7e393-079a-48ff-8b54-ef31f586c10c", new DateTime(2024, 11, 26, 9, 26, 32, 486, DateTimeKind.Utc).AddTicks(4483), "AQAAAAIAAYagAAAAEL+1P9hfm3iwkUXsXoVlvUEIbQmdqHvOAOQbRX5p9WJVQ4gDF6NdU5w0HBafJgNxAQ==", new DateTime(2024, 11, 26, 9, 26, 32, 486, DateTimeKind.Utc).AddTicks(4487) });

            migrationBuilder.CreateIndex(
                name: "IX_CateProducts_CateProductId",
                table: "CateProducts",
                column: "CateProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "CateProducts");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7b72a55e-0189-4665-87ab-b8c4a44e00f0",
                columns: new[] { "ConcurrencyStamp", "CreateAt", "PasswordHash", "UpdateAt" },
                values: new object[] { "f3054803-0cfc-4502-8528-3c1b9539f89d", new DateTime(2024, 11, 26, 3, 53, 17, 903, DateTimeKind.Utc).AddTicks(5231), "AQAAAAIAAYagAAAAEL7QVaPNDyBy47hb25A7dtJQNSlBO9H9iCn8E/ina/SeXX6ZXS5IgeQQyDudWOlWGw==", new DateTime(2024, 11, 26, 3, 53, 17, 903, DateTimeKind.Utc).AddTicks(5235) });
        }
    }
}
