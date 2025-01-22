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

        builder.HasKey(k => k.Id)
               .HasName("PK_Order_Id");

        builder.HasMany(x => x.OrderDetail)
               .WithOne(x => x.Order)
               .HasForeignKey(x => x.OrderId)
               .HasConstraintName("FK_Order_OrderDetail_OrderId")
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.OrderStatus)
               .WithMany(x => x.Orders)
               .HasForeignKey(x => x.OrderStatusId)
               .HasConstraintName("FK_Order_OrderStatus_OrderStatusId")
               .OnDelete(DeleteBehavior.Restrict);
    }
}
