using GrowSphere.Domain.Models.ProjectModel;
using GrowSphere.Domain.Models.SkillModel;
using GrowSphere.Domain.Models.UserModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GrowSphere.Infrastructure.Configuration;

public class ProjectMemberConfiguration : IEntityTypeConfiguration<ProjectMember>
{
    public void Configure(EntityTypeBuilder<ProjectMember> builder)
    {
        builder.ToTable("project_member");

        builder.HasKey(pm => new { pm.UserId, pm.ProjectId });
        
        builder.Property(pm => pm.UserId)
            .HasColumnName("user_id")
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value));

        builder.Property(pm => pm.ProjectId)
            .HasColumnName("project_id")
            .HasConversion(
                id => id.Value,
                value => ProjectId.Create(value));

        builder.HasOne(pm => pm.Project)
            .WithMany(u => u.Members)
            .HasForeignKey(us => us.ProjectId);

        builder.HasOne(pm => pm.User)
            .WithMany()
            .HasForeignKey(us => us.UserId);
        
        builder.Property(pm => pm.Role)
            .HasConversion<string>();
    }
}