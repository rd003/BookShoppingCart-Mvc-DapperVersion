using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShoppingCartMvcUI.Migrations
{
    /// <inheritdoc />
    public partial class appliedindexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_OrderDetail_OrderId",
                table: "OrderDetail",
                newName: "IX_Order_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetail_BookId",
                table: "OrderDetail",
                newName: "IX_Order_BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CreateDate",
                table: "Order",
                column: "CreateDate");

            migrationBuilder.CreateIndex(
                name: "IX_Genre_GenreName",
                table: "Genre",
                column: "GenreName");

            migrationBuilder.CreateIndex(
                name: "IX_Book_AuthorName",
                table: "Book",
                column: "AuthorName")
                .Annotation("SqlServer:Include", new[] { "BookName", "Image", "Price" });

            migrationBuilder.CreateIndex(
                name: "IX_Book_BookName",
                table: "Book",
                column: "BookName")
                .Annotation("SqlServer:Include", new[] { "AuthorName", "Image", "Price" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Order_CreateDate",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Genre_GenreName",
                table: "Genre");

            migrationBuilder.DropIndex(
                name: "IX_Book_AuthorName",
                table: "Book");

            migrationBuilder.DropIndex(
                name: "IX_Book_BookName",
                table: "Book");

            migrationBuilder.RenameIndex(
                name: "IX_Order_OrderId",
                table: "OrderDetail",
                newName: "IX_OrderDetail_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_BookId",
                table: "OrderDetail",
                newName: "IX_OrderDetail_BookId");
        }
    }
}
