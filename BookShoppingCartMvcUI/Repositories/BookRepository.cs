using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BookShoppingCartMvcUI.Repositories;
public interface IBookRepository
{
    Task AddBook(Book book);
    Task DeleteBook(Book book);
    Task<Book?> GetBookById(int id);
    Task<IEnumerable<Book>> GetBooks();
    Task UpdateBook(Book book);
}

public class BookRepository : IBookRepository
{
    private readonly IConfiguration _config;
    private readonly string _constr;
    public BookRepository(IConfiguration config)
    {
        _config = config;
        _constr = _config.GetConnectionString("DefaultConnection");
    }

    public async Task AddBook(Book book)
    {
        IDbConnection connection = new SqlConnection(_constr);
        string sql = @"
                insert into
                Book (BookName,AuthorName,Price,Image,GenreId)
                values (@BookName,@AuthorName,@Price,@Image,@GenreId);
            ";
        await connection.ExecuteAsync(sql, book);
    }

    public async Task UpdateBook(Book book)
    {
        IDbConnection connection = new SqlConnection(_constr);
        string sql = @"
               update Book
               set 
                BookName=@BookName,
                AuthorName=@AuthorName,
                Price=@Price,
                Image=@Image,
                GenreId=@GenreId
               where Id=@Id
            ";
        await connection.ExecuteAsync(sql, book);
    }

    public async Task DeleteBook(Book book)
    {
        IDbConnection connection = new SqlConnection(_constr);
        string sql = @"
                delete from 
                Book
                where Id=@Id 
            ";
        await connection.ExecuteAsync(sql, new { book.Id });
    }

    public async Task<Book?> GetBookById(int id)
    {
        IDbConnection connection = new SqlConnection(_constr);
        string sql = @"
             select
               b.Id,
               b.BookName,
               b.AuthorName,
               b.Price,
               b.Image,
               b.GenreId,
               g.GenreName
             from Book b
             join Genre g
             on b.GenreId = g.Id
             where 
                b.Id=@id
            ";
        Book? book = await connection.QueryFirstOrDefaultAsync<Book>(sql, new { id });
        return book;
    }

    public async Task<IEnumerable<Book>> GetBooks()
    {
        IDbConnection connection = new SqlConnection(_constr);
        string sql = @"
             select
               b.Id,
               b.BookName,
               b.AuthorName,
               b.Price,
               b.Image,
               b.GenreId,
               g.GenreName
             from Book b
             join Genre g
             on b.GenreId = g.Id
             order by 
               b.BookName asc
            ";
        return await connection.QueryAsync<Book>(sql);
    }

}
