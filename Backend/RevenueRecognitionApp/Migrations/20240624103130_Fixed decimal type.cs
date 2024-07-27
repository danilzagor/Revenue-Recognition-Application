using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RevenueRecognition.Migrations
{
    /// <inheritdoc />
    public partial class Fixeddecimaltype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ContractStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Waiting for a payment" },
                    { 2, "Signed" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ContractStatuses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ContractStatuses",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
