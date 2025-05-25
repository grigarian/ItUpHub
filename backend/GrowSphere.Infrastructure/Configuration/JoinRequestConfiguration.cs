using GrowSphere.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GrowSphere.Infrastructure.Configuration;

public class JoinRequestConfiguration : IEntityTypeConfiguration<JoinRequest>
{
    public void Configure(EntityTypeBuilder<JoinRequest> builder)
    {
        builder.ToTable("join_requests");

        builder.HasKey(jr => jr.Id);

        builder.Property(jr => jr.Id)
            .ValueGeneratedNever()
            .HasColumnName("id");

        builder.Property(jr => jr.ProjectId)
            .IsRequired()
            .HasColumnName("project_id");

        builder.Property(jr => jr.UserId)
            .IsRequired()
            .HasColumnName("user_id");

        builder.Property(jr => jr.Message)
            .IsRequired()
            .HasMaxLength(1000)
            .HasColumnName("message");

        builder.Property(jr => jr.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");

        builder.Property(jr => jr.Status)
            .IsRequired()
            .HasColumnName("status")
            .HasConversion<string>(); // Enum as string

        builder.Property(jr => jr.ManagerComment)
            .HasMaxLength(1000)
            .HasColumnName("manager_comment");

        // Можно добавить FK, если нужно (возможно, ты позже добавишь навигационные свойства)
        // builder.HasOne<Project>().WithMany().HasForeignKey(jr => jr.ProjectId);
        // builder.HasOne<User>().WithMany().HasForeignKey(jr => jr.UserId);
    }
}