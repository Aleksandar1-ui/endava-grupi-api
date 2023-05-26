using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace endavaRestApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PricePaid",
                table: "Payments",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Products_PaymentId",
                table: "Products",
                column: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Payments_PaymentId",
                table: "Products",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "PaymentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Payments_PaymentId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_PaymentId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PricePaid",
                table: "Payments");
        }
    }
}
