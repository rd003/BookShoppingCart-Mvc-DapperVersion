using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookShoppingCartMvcUI.Configurations;

public class StockConfiguration : IEntityTypeConfiguration<Stock>
{
    public void Configure(EntityTypeBuilder<Stock> builder)
    {
        builder.ToTable("Stock")
               .HasKey(k => k.Id)
               .HasName("PK_Stock_Id");

        builder.HasOne(x => x.Book)
               .WithOne(x => x.Stock)
               .HasForeignKey<Stock>(k => k.BookId)
               .HasConstraintName("FK_Stock_Book_BookId")
               .OnDelete(DeleteBehavior.Restrict);

    }
}
