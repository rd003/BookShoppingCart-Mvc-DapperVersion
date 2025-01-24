using System.Data;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BookShoppingCartMvcUI.Repositories;

[Authorize(Roles = nameof(Roles.Admin))]
public class ReportRepository : IReportRepository
{
    private readonly IConfiguration _config;
    private readonly string _constr;
    public ReportRepository(IConfiguration config)
    {
        _config = config;
        _constr = _config.GetConnectionString("DefaultConnection");
    }

    public async Task<IEnumerable<TopNSoldBookModel>> GetTopNSellingBooksByDate(DateTime startDate, DateTime endDate, int n = 5)
    {
        IDbConnection connection = new SqlConnection(_constr);
        string @sql = @"
                    SET NOCOUNT ON;

                    with UnitSold as
                    (
                    select 
                    od.BookId, 
                    SUM(od.Quantity) as TotalUnitSold 
                    from [order] o
                    join OrderDetail od on o.Id = od.OrderId
                    where 
                    o.IsPaid=1 
                    and o.IsDeleted=0 
                    and o.CreateDate between @startDate and @endDate
                    group by od.BookId
                    )

                    select top (@n) 
                    b.BookName,
                    b.AuthorName,
                    b.[Image],
                    us.TotalUnitSold
                    from UnitSold us
                    join [Book] b
                    on us.BookId = b.Id
                    order by us.TotalUnitSold desc
        ";
        var topFiveSoldBooks = await connection.QueryAsync<TopNSoldBookModel>(sql, new
        {
            startDate,
            endDate,
            n
        });
        return topFiveSoldBooks;
    }

}

public interface IReportRepository
{
    Task<IEnumerable<TopNSoldBookModel>> GetTopNSellingBooksByDate(DateTime startDate, DateTime endDate, int n);
}