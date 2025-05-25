using GrowSphere.Domain.Models.CategoryModel;
using GrowSphere.Domain.Models.Share;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GrowSphere.Infrastructure.Configuration;

public class CategoryConfiguration :IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("category");
        
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasConversion(
            id => id.Value,
            value => CategoryId.Create(value));

        builder.ComplexProperty(c => c.Title, t =>
        {
            t.Property(p => p.Value)
                .IsRequired()
                .HasMaxLength(Title.TITLE_MAX_LENGHT)
                .HasColumnName("title");
        });
    }
}