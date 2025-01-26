using System.Data;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BookShoppingCartMvcUI.Repositories
{
    public class UserOrderRepository : IUserOrderRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;

        private readonly IConfiguration _config;
        private readonly string _constr;


        public UserOrderRepository(ApplicationDbContext db,
            UserManager<IdentityUser> userManager,
             IHttpContextAccessor httpContextAccessor,
             IConfiguration config)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _config = config;
            _constr = _config.GetConnectionString("DefaultConnection");
        }

        public async Task ChangeOrderStatus(UpdateOrderStatusModel data)
        {
            using IDbConnection connection = new SqlConnection(_constr);
            string sql = @"
                            IF NOT EXISTS(SELECT 1 FROM [Order] where Id=@OrderId)
                            BEGIN
                                RAISERROR('Order with id %d does not exist', 16, 1, @OrderId);
                                RETURN;
                            END

                            UPDATE [Order]
                            SET OrderStatusId=@OrderStatusId
                            WHERE Id = @OrderId";
            await connection.ExecuteAsync(sql, new { data.OrderId, data.OrderStatusId });
        }

        public async Task<Order?> GetOrderById(int id)
        {
            return await _db.Orders.FindAsync(id);
        }

        public async Task<IEnumerable<OrderStatus>> GetOrderStatuses()
        {
            return await _db.orderStatuses.ToListAsync();
        }

        public async Task TogglePaymentStatus(int orderId)
        {
            var order = await _db.Orders.FindAsync(orderId);
            if (order == null)
            {
                throw new InvalidOperationException($"order withi id:{orderId} does not found");
            }
            order.IsPaid = !order.IsPaid;
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> UserOrders(bool getAll = false)
        {
            var orders = _db.Orders
                           .Include(x => x.OrderStatus)
                           .Include(x => x.OrderDetail)
                           .ThenInclude(x => x.Book)
                           .ThenInclude(x => x.Genre).AsQueryable();
            if (!getAll)
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not logged-in");
                orders = orders.Where(a => a.UserId == userId);
                return await orders.ToListAsync();
            }

            return await orders.ToListAsync();
        }

        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }
    }
}
