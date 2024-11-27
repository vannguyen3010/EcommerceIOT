using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.API.Migrations
{
    /// <inheritdoc />
    public partial class TableCartItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImageFilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameSlug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7b72a55e-0189-4665-87ab-b8c4a44e00f0",
                columns: new[] { "ConcurrencyStamp", "CreateAt", "PasswordHash", "UpdateAt" },
                values: new object[] { "382a0bd7-e27d-4d86-a4e0-d3302690c4b9", new DateTime(2024, 11, 27, 5, 51, 7, 753, DateTimeKind.Utc).AddTicks(56), "AQAAAAIAAYagAAAAEFm0CL6z4UdO+4kLq3AW2HxIOsPz1X+k96ckEAYRQpWu0Z683R7effOqMdGVtHr3pA==", new DateTime(2024, 11, 27, 5, 51, 7, 753, DateTimeKind.Utc).AddTicks(60) });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7b72a55e-0189-4665-87ab-b8c4a44e00f0",
                columns: new[] { "ConcurrencyStamp", "CreateAt", "PasswordHash", "UpdateAt" },
                values: new object[] { "e7e7e393-079a-48ff-8b54-ef31f586c10c", new DateTime(2024, 11, 26, 9, 26, 32, 486, DateTimeKind.Utc).AddTicks(4483), "AQAAAAIAAYagAAAAEL+1P9hfm3iwkUXsXoVlvUEIbQmdqHvOAOQbRX5p9WJVQ4gDF6NdU5w0HBafJgNxAQ==", new DateTime(2024, 11, 26, 9, 26, 32, 486, DateTimeKind.Utc).AddTicks(4487) });
        }
    }
}
