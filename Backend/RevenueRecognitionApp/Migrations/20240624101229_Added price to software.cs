using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RevenueRecognition.Migrations
{
    /// <inheritdoc />
    public partial class Addedpricetosoftware : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Software",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Software");
        }
    }
}
