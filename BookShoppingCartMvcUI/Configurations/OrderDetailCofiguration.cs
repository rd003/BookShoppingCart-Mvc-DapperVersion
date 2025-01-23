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

        builder.HasKey(k => k.Id);

        builder.HasOne(a => a.Order)
               .WithMany(a => a.OrderDetail)
               .HasForeignKey(a => a.OrderId)
               .HasConstraintName("FK_OrderDetail_Order_OrderId")
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Book)
               .WithMany(a => a.OrderDetail)
               .HasForeignKey(a => a.BookId)
               .HasConstraintName("FK_OrderDetail_Book_BookId")
               .OnDelete(DeleteBehavior.Restrict);


    }
}
