using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataSizeAndCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("145b5f07-4c0c-467f-b7dc-6de42bf99f0d"), "Balo" },
                    { new Guid("33e2f8fb-ab07-462d-931f-e12e0b2899e7"), "Jean" },
                    { new Guid("3b82e3bb-9e75-47b0-b746-44ad479bb9fc"), "Shirt" },
                    { new Guid("3f3caf91-5d67-4634-accf-94d38a27f273"), "Accessory" },
                    { new Guid("4349a1bb-863a-4d53-8ca5-038f4580148a"), "Polo" },
                    { new Guid("55ea8b18-22b6-4c5a-b4b1-57a5cda56fec"), "Short" },
                    { new Guid("a772851b-75c0-4a5e-bfe9-656be931a214"), "Hoodie" },
                    { new Guid("b0139787-2c30-4006-bfed-9180113debb1"), "Jacket" },
                    { new Guid("c2a5dbb4-45df-4af1-b832-1b75d4f9ac6a"), "T-Shirt" }
                });

            migrationBuilder.InsertData(
                table: "Sizes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("2b20faec-76f9-4554-a551-d48069b1f7f3"), "XL" },
                    { new Guid("2e14040a-1cfe-4831-87ac-838dce132dbf"), "free-size" },
                    { new Guid("338b7be5-0ef6-4ca7-ad53-22ded93e6abd"), "M" },
                    { new Guid("580ad9d5-28ed-46c9-b2dd-3442fb19cfda"), "S" },
                    { new Guid("c9d2db7e-e95f-4a36-9ad3-68c7110d5ded"), "XXL" },
                    { new Guid("e63de726-61b6-49aa-ad5c-e555b20297c5"), "L" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("145b5f07-4c0c-467f-b7dc-6de42bf99f0d"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("33e2f8fb-ab07-462d-931f-e12e0b2899e7"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("3b82e3bb-9e75-47b0-b746-44ad479bb9fc"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("3f3caf91-5d67-4634-accf-94d38a27f273"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("4349a1bb-863a-4d53-8ca5-038f4580148a"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("55ea8b18-22b6-4c5a-b4b1-57a5cda56fec"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("a772851b-75c0-4a5e-bfe9-656be931a214"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("b0139787-2c30-4006-bfed-9180113debb1"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("c2a5dbb4-45df-4af1-b832-1b75d4f9ac6a"));

            migrationBuilder.DeleteData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: new Guid("2b20faec-76f9-4554-a551-d48069b1f7f3"));

            migrationBuilder.DeleteData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: new Guid("2e14040a-1cfe-4831-87ac-838dce132dbf"));

            migrationBuilder.DeleteData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: new Guid("338b7be5-0ef6-4ca7-ad53-22ded93e6abd"));

            migrationBuilder.DeleteData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: new Guid("580ad9d5-28ed-46c9-b2dd-3442fb19cfda"));

            migrationBuilder.DeleteData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: new Guid("c9d2db7e-e95f-4a36-9ad3-68c7110d5ded"));

            migrationBuilder.DeleteData(
                table: "Sizes",
                keyColumn: "Id",
                keyValue: new Guid("e63de726-61b6-49aa-ad5c-e555b20297c5"));
        }
    }
}
