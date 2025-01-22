using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShoppingCartMvcUI.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Book")
               .HasIndex(b => b.BookName)
               .IncludeProperties(p => new
               {
                   p.AuthorName,
                   p.Image,
                   p.Price
               })
               .HasDatabaseName("IX_Book_BookName");

        builder.HasIndex(e => e.AuthorName)
               .HasDatabaseName("IX_Book_AuthorName")
               .IncludeProperties(p => new
               {
                   p.BookName,
                   p.Image,
                   p.Price
               });

        builder.HasIndex(e => e.GenreId)
               .HasDatabaseName("IX_Book_GenreId");
    }
}
