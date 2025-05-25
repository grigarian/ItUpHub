using GrowSphere.Domain.Models.ProjectModel;
using GrowSphere.Domain.Models.UserModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GrowSphere.Infrastructure.Configuration;

public class UserProjectConfiguration : IEntityTypeConfiguration<UserProject>
{
    public void Configure(EntityTypeBuilder<UserProject> builder)
    {
        builder.ToTable("user_project");

        builder.HasKey(us => new { us.UserId, us.ProjectId });
        
        builder.Property(us => us.UserId)
            .HasColumnName("user_id")
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value));

        builder.Property(us => us.ProjectId)
            .HasColumnName("project_id")
            .HasConversion(
                id => id.Value,
                value => ProjectId.Create(value));

        builder.HasOne(us => us.User)
            .WithMany(u => u.Projects)
            .HasForeignKey(us => us.UserId);

        builder.HasOne(us => us.Project)
            .WithMany()
            .HasForeignKey(us => us.ProjectId);
    }
}