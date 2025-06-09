using GrowSphere.Domain.Models.ProjectMessage;
using GrowSphere.Domain.Models.ProjectModel;
using GrowSphere.Domain.Models.UserModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GrowSphere.Infrastructure.Configuration;

public class ProjectMessageConfiguration: IEntityTypeConfiguration<ProjectMessage>
{
    public void Configure(EntityTypeBuilder<ProjectMessage> builder)
    {
        builder.ToTable("project_messages");

        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => ProjectMessageId.Create(value));
        
        builder.Property(pm => pm.ProjectId)
            .HasColumnName("project_id")
            .IsRequired();
        
        builder.Property(p => p.ProjectId)
            .HasConversion(
                id => id.Value,
                value => ProjectId.Create(value));

        builder.Property(pm => pm.SenderId)
            .HasColumnName("sender_id")
            .IsRequired();
        
        builder.Property(p => p.SenderId)
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value));
        
        builder.Property(pm => pm.Content)
            .HasColumnName("content")
            .IsRequired()
            .HasMaxLength(2000); // Максимум 2000 символов

        builder.Property(pm => pm.SentAt)
            .HasColumnName("sent_at")
            .IsRequired();

        builder.HasOne(pm => pm.Project)
            .WithMany()
            .HasForeignKey(pm => pm.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pm => pm.Sender)
            .WithMany()
            .HasForeignKey(pm => pm.SenderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}