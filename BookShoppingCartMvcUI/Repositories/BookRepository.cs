using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BookShoppingCartMvcUI.Repositories
{
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
            await connection.ExecuteAsync(sql);
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
            ";
            await connection.ExecuteAsync(sql, book);
        }

        public async Task DeleteBook(Book book)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        public async Task<Book?> GetBookById(int id) => await _context.Books.FindAsync(id);

        public async Task<IEnumerable<Book>> GetBooks() => await _context.Books.Include(a => a.Genre).ToListAsync();
    }
}
