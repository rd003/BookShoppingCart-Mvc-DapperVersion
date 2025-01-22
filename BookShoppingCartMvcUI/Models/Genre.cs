using System.ComponentModel.DataAnnotations;

namespace BookShoppingCartMvcUI.Models
{
    public class Genre
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string GenreName { get; set; } = string.Empty;
        public List<Book> Books { get; set; } = [];
    }
}
