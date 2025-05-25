using GrowSphere.Domain.Models.CategoryModel;
using GrowSphere.Domain.Models.ProjectModel;
using GrowSphere.Domain.Models.Share;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GrowSphere.Infrastructure.Configuration;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("project");
        
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => ProjectId.Create(value));

        builder.ComplexProperty(p => p.Title, t =>
        {
            t.Property(p => p.Value)
                .IsRequired()
                .HasMaxLength(Title.TITLE_MAX_LENGHT)
                .HasColumnName("title");
        });
        
        builder.ComplexProperty(p => p.Description, d =>
        {
            d.Property(p => p.Value)
                .IsRequired()
                .HasMaxLength(Description.DESCRIPTION_MAX_LENGHT)
                .HasColumnName("description");
        });

        builder.HasOne(p => p.Category)
            .WithMany(c => c.Projects)
            .HasForeignKey(p => p.CategoryId)
            .IsRequired();
        
        builder.HasMany(p => p.Members)
            .WithOne(pm => pm.Project)
            .HasForeignKey(pm => pm.ProjectId)
            .IsRequired(false);
        
        builder.HasMany(p => p.Issues)
            .WithOne(i => i.Project)
            .HasForeignKey(i => i.ProjectId)
            .IsRequired();
        
        builder.ComplexProperty(p => p.Status, s =>
        {
            s.Property(p => p.Value)
                .IsRequired()
                .HasColumnName("status");
        });
        
        builder.Property(p => p.StartDate)
            .IsRequired()
            .HasColumnName("start_date");

        builder.Property(p => p.EndDate)
            .HasColumnName("end_date")
            .IsRequired(false);
        
        builder.Property(p => p.UpdateDate)
            .HasColumnName("update_date")
            .IsRequired();
        
        builder.Property(p => p.CreationDate)
            .HasColumnName("creation_date")
            .IsRequired();
    }
}