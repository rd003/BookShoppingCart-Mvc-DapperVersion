using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShoppingCartMvcUI.Configurations;

public class CartDetailConfiguration : IEntityTypeConfiguration<CartDetail>
{
    public void Configure(EntityTypeBuilder<CartDetail> builder)
    {
        builder.ToTable("CartDetail")
               .HasIndex(e => e.ShoppingCartId)
               .HasDatabaseName("IX_CartDetail_ShoppingCartId");

        builder.HasKey(k => k.Id)
               .HasName("PK_CartDetail_Id");

        builder.HasOne(a => a.Book)
               .WithMany(a => a.CartDetail)
               .HasForeignKey(a => a.BookId)
               .HasConstraintName("FK_CartDetail_Book_BookId")
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ShoppingCart)
               .WithMany(x => x.CartDetails)
               .HasForeignKey(a => a.ShoppingCartId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
