using System.ComponentModel.DataAnnotations;

namespace BookShoppingCartMvcUI.Models
{
    public class OrderStatus
    {
        public int Id { get; set; }
        [Required]
        public int StatusId { get; set; }
        [Required, MaxLength(20)]
        public string? StatusName { get; set; }

        public ICollection<Order> Orders { get; set; } = [];
    }
}
