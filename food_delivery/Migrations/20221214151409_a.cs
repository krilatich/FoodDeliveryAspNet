using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace fooddelivery.Migrations
{
    /// <inheritdoc />
    public partial class a : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: new Guid("0f8d3ddb-3b83-4d7d-91f7-3b1e5d0758a5"));

            migrationBuilder.DeleteData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: new Guid("2a47a599-eb46-442b-b6aa-6c0fbdfc8118"));

            migrationBuilder.DeleteData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: new Guid("4e918350-a32e-46c0-9a9c-d40bfaff4cc0"));

            migrationBuilder.DeleteData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: new Guid("59fb1ce6-13d3-4364-b839-ff4d46a73494"));

            migrationBuilder.DeleteData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: new Guid("9b57498b-1c18-4dcf-9c75-c4699f8dd1ce"));

            migrationBuilder.DeleteData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: new Guid("c528b617-97da-41ae-ad1f-3ce93c9b2599"));

            migrationBuilder.DeleteData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: new Guid("cfdabed6-1225-45db-90cb-367bfc71af81"));

            migrationBuilder.CreateTable(
                name: "Rating",
                columns: table => new
                {
                    DishID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RatingScore = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rating", x => new { x.UserID, x.DishID });
                });

            migrationBuilder.InsertData(
                table: "Dishes",
                columns: new[] { "Id", "description", "dishCategory", "image", "name", "price", "rating", "vegetarian" },
                values: new object[,]
                {
                    { new Guid("2f7dcc0f-84b1-46ea-8cb5-a226f0a04b7e"), "5Wok description", 0, null, "5Wok", 1100.0, null, false },
                    { new Guid("66ebeb1d-e48e-4771-9e2a-8794d37e0630"), "firstWok description", 0, null, "firstWok", 500.0, null, false },
                    { new Guid("76010319-021f-4711-8e9b-0b059f4ee5d6"), "fourthWok description", 0, null, "fourthWok", 300.0, null, false },
                    { new Guid("79d69072-2292-4472-aa24-b03a16f0aed8"), "7Wok description", 0, null, "7Wok", 830.0, null, true },
                    { new Guid("adb44e0b-790a-4d70-a5f8-6505cf6fdda3"), "thirdWok description", 0, null, "thirdWok", 600.0, null, false },
                    { new Guid("ce920590-b84d-469e-83a8-c5281addbee5"), "6Wok description", 0, null, "6Wok", 650.0, null, true },
                    { new Guid("f8dc95d6-c4b7-4121-a2b7-e0c7f2099ba9"), "secondWok description", 0, null, "secondWok", 750.0, null, true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rating");

            migrationBuilder.DeleteData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: new Guid("2f7dcc0f-84b1-46ea-8cb5-a226f0a04b7e"));

            migrationBuilder.DeleteData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: new Guid("66ebeb1d-e48e-4771-9e2a-8794d37e0630"));

            migrationBuilder.DeleteData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: new Guid("76010319-021f-4711-8e9b-0b059f4ee5d6"));

            migrationBuilder.DeleteData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: new Guid("79d69072-2292-4472-aa24-b03a16f0aed8"));

            migrationBuilder.DeleteData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: new Guid("adb44e0b-790a-4d70-a5f8-6505cf6fdda3"));

            migrationBuilder.DeleteData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: new Guid("ce920590-b84d-469e-83a8-c5281addbee5"));

            migrationBuilder.DeleteData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: new Guid("f8dc95d6-c4b7-4121-a2b7-e0c7f2099ba9"));

            migrationBuilder.InsertData(
                table: "Dishes",
                columns: new[] { "Id", "description", "dishCategory", "image", "name", "price", "rating", "vegetarian" },
                values: new object[,]
                {
                    { new Guid("0f8d3ddb-3b83-4d7d-91f7-3b1e5d0758a5"), "7Wok description", 0, null, "7Wok", 830.0, null, true },
                    { new Guid("2a47a599-eb46-442b-b6aa-6c0fbdfc8118"), "firstWok description", 0, null, "firstWok", 500.0, null, false },
                    { new Guid("4e918350-a32e-46c0-9a9c-d40bfaff4cc0"), "secondWok description", 0, null, "secondWok", 750.0, null, true },
                    { new Guid("59fb1ce6-13d3-4364-b839-ff4d46a73494"), "6Wok description", 0, null, "6Wok", 650.0, null, true },
                    { new Guid("9b57498b-1c18-4dcf-9c75-c4699f8dd1ce"), "5Wok description", 0, null, "5Wok", 1100.0, null, false },
                    { new Guid("c528b617-97da-41ae-ad1f-3ce93c9b2599"), "thirdWok description", 0, null, "thirdWok", 600.0, null, false },
                    { new Guid("cfdabed6-1225-45db-90cb-367bfc71af81"), "fourthWok description", 0, null, "fourthWok", 300.0, null, false }
                });
        }
    }
}
