using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShoppingCartMvcUI.Configurations;

public class OrderDetailCofiguration : IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(EntityTypeBuilder<OrderDetail> builder)
    {
        builder.ToTable("OrderDetail")
               .HasIndex(e => e.OrderId)
               .HasDatabaseName("IX_Order_OrderId");

        builder.HasIndex(e => e.BookId)
               .HasDatabaseName("IX_Order_BookId");
    }
}
