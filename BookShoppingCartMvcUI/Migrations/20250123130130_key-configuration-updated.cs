using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShoppingCartMvcUI.Migrations
{
    /// <inheritdoc />
    public partial class keyconfigurationupdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_Stock_BookId",
                table: "Stock",
                newName: "IX_Book_BookId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_Book_BookId",
                table: "Stock",
                newName: "IX_Stock_BookId");
        }
    }
}
