using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookShoppingCartMvcUI.Configurations;

public class StockConfiguration : IEntityTypeConfiguration<Stock>
{
       public void Configure(EntityTypeBuilder<Stock> builder)
       {
              builder.ToTable("Stock", b => b.HasCheckConstraint("CK_Stock_Quantity", "Quantity>=0"))
                     .HasKey(k => k.Id)
                     .HasName("PK_Stock_Id");

              builder.HasIndex(a => a.BookId)
                     .IsUnique(true)
                     .HasDatabaseName("IX_Stock_BookId");

              builder.HasOne(x => x.Book)
                     .WithOne(x => x.Stock)
                     .HasForeignKey<Stock>(k => k.BookId)
                     .HasConstraintName("FK_Stock_Book_BookId")
                     .OnDelete(DeleteBehavior.Restrict);

       }
}
