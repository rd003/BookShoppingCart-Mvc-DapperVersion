using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShoppingCartMvcUI.Configurations;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.ToTable("Genre")
               .HasIndex(g => g.GenreName);

        builder.HasKey(k => k.Id)
               .HasName("PK_Genre_Id");

    }
}
