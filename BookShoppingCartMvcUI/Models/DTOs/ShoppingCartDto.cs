namespace BookShoppingCartMvcUI.Models.DTOs;

public class ShoppingCartDto
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public List<CartDetailDto> CartDetails { get; set; }
    public decimal TotalPrice => CartDetails?.Sum(cd => cd.TotalPrice) ?? 0;
}

public class CartDetailDto
{
    public int BookId { get; set; }
    public int Quantity { get; set; }
    public BookDto Book { get; set; }
    public decimal TotalPrice => Book?.Price * Quantity ?? 0;
}

public class BookDto
{
    public string BookName { get; set; }
    public decimal Price { get; set; }
    public string Image { get; set; }
    public GenreDto Genre { get; set; }
    public StockDto Stock { get; set; }
}

public class GenreDto
{
    public string GenreName { get; set; }
}

public class StockDto
{
    public int StockQuantity { get; set; }
}