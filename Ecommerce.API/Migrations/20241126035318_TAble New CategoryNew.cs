using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.API.Migrations
{
    /// <inheritdoc />
    public partial class TAbleNewCategoryNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoryNews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameSlug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    ParentCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryNews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryNews_CategoryNews_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "CategoryNews",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameSlug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileExtension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileSizeInBytes = table.Column<long>(type: "bigint", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsHot = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                    table.ForeignKey(
                        name: "FK_News_CategoryNews_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "CategoryNews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7b72a55e-0189-4665-87ab-b8c4a44e00f0",
                columns: new[] { "ConcurrencyStamp", "CreateAt", "PasswordHash", "UpdateAt" },
                values: new object[] { "f3054803-0cfc-4502-8528-3c1b9539f89d", new DateTime(2024, 11, 26, 3, 53, 17, 903, DateTimeKind.Utc).AddTicks(5231), "AQAAAAIAAYagAAAAEL7QVaPNDyBy47hb25A7dtJQNSlBO9H9iCn8E/ina/SeXX6ZXS5IgeQQyDudWOlWGw==", new DateTime(2024, 11, 26, 3, 53, 17, 903, DateTimeKind.Utc).AddTicks(5235) });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryNews_ParentCategoryId",
                table: "CategoryNews",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_News_CategoryId",
                table: "News",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "CategoryNews");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7b72a55e-0189-4665-87ab-b8c4a44e00f0",
                columns: new[] { "ConcurrencyStamp", "CreateAt", "PasswordHash", "UpdateAt" },
                values: new object[] { "8f5773b9-4a00-4cf8-96eb-5dd0237fbf20", new DateTime(2024, 11, 26, 3, 50, 3, 998, DateTimeKind.Utc).AddTicks(6982), "AQAAAAIAAYagAAAAEHDIU7lDy4hWmTgrJiE29X9M0wMnoE9b6qXoOzhPNfcyamxcJO/TVfFTSuRQVFOzaQ==", new DateTime(2024, 11, 26, 3, 50, 3, 998, DateTimeKind.Utc).AddTicks(6986) });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryIntroduces_ParentCategoryId",
                table: "CategoryIntroduces",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Introduces_CategoryId",
                table: "Introduces",
                column: "CategoryId");
        }
    }
}
