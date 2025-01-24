using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShoppingCartMvcUI.Migrations
{
    /// <inheritdoc />
    public partial class keyconfigurationupdated2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Book_BookId",
                table: "Stock");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_BookId",
                table: "Stock",
                column: "BookId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Stock_BookId",
                table: "Stock");

            migrationBuilder.CreateIndex(
                name: "IX_Book_BookId",
                table: "Stock",
                column: "BookId",
                unique: true);
        }
    }
}
