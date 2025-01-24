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

    public async Task<IEnumerable<Book>> GetBooks(string sTerm = "", int genreId = 0)
    {
        sTerm = sTerm.ToLower();

        IDbConnection conn = new SqlConnection(constr);

        string sql = @"SELECT
                            b.Id,
                            b.[Image],
                            b.AuthorName,
                            b.BookName,
                            b.GenreId,
                            b.Price,
                            g.GenreName,
                            COALESCE(s.Quantity,0) as Quantity
                        FROM Book b
                            inner join Genre g
                            on b.GenreId = g.Id
                            left outer join Stock s
                            on b.Id = s.Id
                        Where 
                            (@search_term = '' OR b.BookName LIKE @search_term + '%')
                            AND (@genre_id = 0 OR b.GenreId = @genre_id) 
                        ORDER BY b.BookName ASC
                    ";
        IEnumerable<Book> books = await conn.QueryAsync<Book>(sql, new { genre_id = genreId, search_term = sTerm });
        return books;

    }
}
