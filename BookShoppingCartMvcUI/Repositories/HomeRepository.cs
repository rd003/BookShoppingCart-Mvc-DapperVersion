using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BookShoppingCartMvcUI.Repositories;

public class HomeRepository : IHomeRepository
{
    private readonly IConfiguration _config;
    private readonly string constr;


    public HomeRepository(IConfiguration config)
    {
        _config = config;
        constr = _config.GetConnectionString("DefaultConnection");
    }

    public async Task<IEnumerable<Book>> GetBooks(string sTerm = "show_all", int genreId = 0)
    {
        sTerm = sTerm.ToLower();
        using IDbConnection conn = new SqlConnection(constr);
        IEnumerable<Book> books = await conn.QueryAsync<Book>("USP_GetBooksForHomePage", new { genre_id = genreId, search_term = sTerm });
        return books;

    }
}
