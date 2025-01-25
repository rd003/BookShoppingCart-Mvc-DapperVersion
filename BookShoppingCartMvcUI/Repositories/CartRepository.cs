using System.Data;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BookShoppingCartMvcUI.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;
        private readonly string _constr;

        public CartRepository(IHttpContextAccessor httpContextAccessor,
            UserManager<IdentityUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _config = config;
            _constr = _config.GetConnectionString("DefaultConnection");
        }


        private IDbConnection CreateConnection() =>
        new SqlConnection(_constr);

        public async Task<int> AddItem(int bookId, int quantity)
        {
            string userId = GetUserId();  // getting it from http context

            IDbConnection connection = new SqlConnection(_constr);

            var parameters = new DynamicParameters();
            parameters.Add("@BookId", bookId);
            parameters.Add("@Quantity", quantity);
            parameters.Add("@UserId", userId);
            parameters.Add("@ReturnedValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await connection.ExecuteAsync("AddItemToCart", parameters, commandType: CommandType.StoredProcedure);

            int cartItemCount = parameters.Get<int>("@ReturnedValue");

            return cartItemCount;
        }


        public async Task<int> RemoveItem(int bookId)
        {
            string userId = GetUserId();
            IDbConnection connection = new SqlConnection(_constr);

            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);
            parameters.Add("@BookId", bookId);
            parameters.Add("@CartItemCount", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await connection.ExecuteAsync("RemoveCartItem", parameters, commandType: CommandType.StoredProcedure);

            int cartItemCount = parameters.Get<int>("@CartItemCount");
            return cartItemCount;
        }

        public async Task<ShoppingCartDto> GetUserCart()
        {
            var userId = GetUserId();
            if (userId == null)
                throw new InvalidOperationException("Invalid userid");

            IDbConnection connection = new SqlConnection(_constr);
            string query = @"
        SELECT 
            sc.Id as Id, 
            sc.UserId as UserId,
            cd.BookId,
            cd.Quantity,
            b.BookName, 
            b.Price, 
            b.Image,
            g.GenreName,
            s.Quantity as StockQuantity
        FROM ShoppingCart sc
        LEFT JOIN CartDetail cd ON sc.Id = cd.ShoppingCartId
        LEFT JOIN Book b ON cd.BookId = b.Id
        LEFT JOIN Genre g ON b.GenreId = g.Id
        LEFT JOIN Stock s ON b.Id = s.BookId
        WHERE sc.UserId = @UserId";

            var cartDictionary = new Dictionary<int, ShoppingCartDto>();

            var result = await connection.QueryAsync<ShoppingCartDto, CartDetailDto, BookDto, GenreDto, StockDto, ShoppingCartDto>(
                query,
                (cart, cartDetail, book, genre, stock) =>
                {
                    if (!cartDictionary.TryGetValue(cart.Id, out var cartEntry))
                    {
                        cartEntry = cart;
                        cartEntry.CartDetails = new List<CartDetailDto>();
                        cartDictionary.Add(cartEntry.Id, cartEntry);
                    }

                    if (cartDetail != null && book != null)
                    {
                        cartDetail.Book = book;
                        book.Genre = genre;
                        book.Stock = stock;
                        cartEntry.CartDetails.Add(cartDetail);
                    }

                    return cartEntry;
                },
                new { UserId = userId },
                splitOn: "BookId,BookName,GenreName,StockQuantity"
            );

            return cartDictionary.Values.FirstOrDefault();
        }

        public async Task<ShoppingCart> GetCart(string userId)
        {
            IDbConnection connection = new SqlConnection(_constr);
            string sql = @"select * from ShoppingCart where UserId = @userId ";
            var cart = await connection.QueryFirstAsync<ShoppingCart>(sql, new { userId });
            return cart;
        }

        public async Task<int> GetCartItemCount(string userId = "")
        {
            userId = GetUserId();

            IDbConnection connection = new SqlConnection(_constr);
            string sql = @"select 
                            Count(1) as TotalCount
                           from ShoppingCart s
                           inner join CartDetail cd
                           on s.Id = cd.ShoppingCartId
                           where s.UserId = @userId;
                         ";
            int totalCount = await connection.QueryFirstAsync<int>(sql, new { userId });
            return totalCount;
        }

        public async Task DoCheckout(CheckoutModel model)
        {
            model.UserId = GetUserId();
            IDbConnection connection = new SqlConnection(_constr);
            await connection.ExecuteAsync("Checkout", param: model, commandType: CommandType.StoredProcedure);
        }

        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }


    }
}
