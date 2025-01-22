using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShoppingCartMvcUI.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {

        builder.ToTable("Order")
               .HasIndex(e => e.CreateDate)
               .HasDatabaseName("IX_Order_CreateDate");

        builder.HasIndex(e => e.OrderStatusId)
               .HasDatabaseName("IX_Order_OrderStatusId");
    }
}
