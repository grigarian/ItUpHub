using GrowSphere.Domain.Models.Share;
using GrowSphere.Domain.Models.SkillModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GrowSphere.Infrastructure.Configuration;

public class SkillConfiguration : IEntityTypeConfiguration<Skill>
{
    public void Configure(EntityTypeBuilder<Skill> builder)
    {
        builder.ToTable("skill");
        
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasConversion(
                id => id.Value,
                value => SkillId.Create(value));

        builder.ComplexProperty(c => c.Title, t =>
        {
            t.Property(p => p.Value)
                .IsRequired()
                .HasMaxLength(Title.TITLE_MAX_LENGHT)
                .HasColumnName("title");
        });
    }
}