using GrowSphere.Domain.Models.IssueModel;
using GrowSphere.Domain.Models.Share;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GrowSphere.Infrastructure.Configuration;

public class IssueConfiguration : IEntityTypeConfiguration<Issue>
{
    public void Configure(EntityTypeBuilder<Issue> builder)
    {
        builder.ToTable("issue");
        
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .HasConversion(
                id => id.Value,
                value => IssueId.Create(value));

        builder.ComplexProperty(i => i.Title, t =>
        {
            t.Property(p => p.Value)
                .IsRequired()
                .HasMaxLength(Title.TITLE_MAX_LENGHT)
                .HasColumnName("title");
        });
        
        builder.ComplexProperty(i => i.Description, d =>
        {
            d.Property(p => p.Value)
                .IsRequired()
                .HasMaxLength(Description.DESCRIPTION_MAX_LENGHT)
                .HasColumnName("description");
        });
        
        builder.ComplexProperty(i => i.Picture, p =>
        {
            p.Property(p => p.Path)
                .HasColumnName("picture_path")
                .IsRequired(false);
            p.Property(p => p.MimeType)
                .HasColumnName("mime_type")
                .IsRequired(false);
        });
        
        builder.Property(i => i.CompleteDate)
            .IsRequired()
            .HasColumnName("complete_date");

        builder.ComplexProperty(i => i.Status, s =>
        {
            s.Property(p => p.Value)
                .IsRequired()
                .HasColumnName("status");
        });
        
        builder.HasOne(i => i.AssignerUser)
            .WithMany()
            .HasForeignKey(i => i.AssignerUserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}