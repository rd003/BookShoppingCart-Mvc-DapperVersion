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
    }
}
