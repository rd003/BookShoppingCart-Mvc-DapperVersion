using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BookShoppingCartMvcUI.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly IConfiguration _config;
        private readonly string _constr;

        public StockRepository(IConfiguration config)
        {
            _config = config;
            _constr = _config.GetConnectionString("DefaultConnection");
        }

        public async Task<Stock?> GetStockByBookId(int bookId)
        {
            IDbConnection connection = new SqlConnection(_constr);
            string sql = @"
                           SELECT 
                            s.Id,
                            s.BookId,
                            s.Quantity
                           FROM
                           Stock s
                           WHERE s.BookId=@bookId;
            ";
            Stock? stock = await connection.QueryFirstOrDefaultAsync<Stock>(sql, new { bookId });
            return stock;
        }

        public async Task<IEnumerable<StockDisplayModel>> GetStocks(string sTerm = "")
        {
            string sql = @"
                         SELECT 
                          b.Id as BookId,
                          b.BookName,
                          COALESCE(s.Quantity,0) as Quantity
                         FROM
                         Book b
                         LEFT JOIN Stock s
                         on b.Id=s.BookId
                         WHERE @sTerm = '' or b.BookName LIKE @sTerm+'%';
            ";
            IDbConnection connection = new SqlConnection(_constr);
            var stocks = await connection.QueryAsync<StockDisplayModel>(sql, new { sTerm });
            return stocks;
        }

        public async Task ManageStock(StockDTO stockToManage)
        {
            // if there is no stock for given book id, then add new record
            // if there is already stock for given book id, update stock's quantity
            IDbConnection connection = new SqlConnection(_constr);
            string sql = @"
                         IF NOT EXISTS (SELECT 1 FROM Stock Where BookID = @BookId)
                          BEGIN
                            INSERT INTO Stock (BookId,Quantity)
                            VALUES (@BookId,@Quantity);
                          END
                         ELSE
                          BEGIN
                            UPDATE Stock 
                            SET Quantity=@Quantity 
                            WHERE BookId=@BookId;
                          END
                        ";
            await connection.ExecuteAsync(sql, stockToManage);
        }

    }

    public interface IStockRepository
    {
        Task<IEnumerable<StockDisplayModel>> GetStocks(string sTerm = "");
        Task<Stock?> GetStockByBookId(int bookId);
        Task ManageStock(StockDTO stockToManage);
    }
}
