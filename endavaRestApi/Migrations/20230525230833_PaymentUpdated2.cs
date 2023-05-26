using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace endavaRestApi.Migrations
{
    /// <inheritdoc />
    public partial class PaymentUpdated2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Payments");

            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                table: "Products",
                type: "int",
                nullable: true);

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
    }
}
