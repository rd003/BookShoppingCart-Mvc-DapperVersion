using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShoppingCartMvcUI.Migrations
{
    /// <inheritdoc />
    public partial class keyconfigurationupdated3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Stock_BookId",
                table: "Stock");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_BookId",
                table: "Stock",
                column: "BookId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Stock_BookId",
                table: "Stock");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_BookId",
                table: "Stock",
                column: "BookId");
        }
    }
}
