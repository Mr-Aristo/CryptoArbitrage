using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PriceData.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FuturePrices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Symbol = table.Column<string>(type: "text", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuturePrices", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "FuturePrices",
                columns: new[] { "Id", "Price", "Symbol", "TimeStamp" },
                values: new object[,]
                {
                    { new Guid("14afa7c6-602e-455d-9c4c-b113c0ba12ae"), 45000.50m, "BTCUSDT_QUARTER", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("e9a5085c-64dc-46e8-9383-4d083732e334"), 3000.25m, "BTCUSDT_BI-QUARTER", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FuturePrices");
        }
    }
}
