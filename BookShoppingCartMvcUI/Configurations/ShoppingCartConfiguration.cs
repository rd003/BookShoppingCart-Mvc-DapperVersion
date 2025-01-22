using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShoppingCartMvcUI.Configurations;

public class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
{
    public void Configure(EntityTypeBuilder<ShoppingCart> builder)
    {
        builder.ToTable("ShoppingCart")
               .HasKey(k => k.Id)
               .HasName("PK_ShoppingCart_Id");

        builder.HasMany(sc => sc.CartDetails)
               .WithOne(cd => cd.ShoppingCart)
               .HasForeignKey(cd => cd.ShoppingCartId)
               .HasConstraintName("FK_ShoppingCart_CartDetail_ShoppingCartId")
               .OnDelete(DeleteBehavior.Restrict);
    }
}
