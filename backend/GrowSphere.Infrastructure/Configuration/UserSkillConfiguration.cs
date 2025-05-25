using GrowSphere.Domain.Models.SkillModel;
using GrowSphere.Domain.Models.UserModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GrowSphere.Infrastructure.Configuration;

public class UserSkillConfiguration : IEntityTypeConfiguration<UserSkill>
{
    public void Configure(EntityTypeBuilder<UserSkill> builder)
    {
        builder.ToTable("user_skill");

        builder.HasKey(us => new { us.UserId, us.SkillId });
        
        builder.Property(us => us.UserId)
            .HasColumnName("user_id")
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value));

        builder.Property(us => us.SkillId)
            .HasColumnName("skill_id")
            .HasConversion(
                id => id.Value,
                value => SkillId.Create(value));

        builder.HasOne(us => us.User)
            .WithMany(u => u.Skills)
            .HasForeignKey(us => us.UserId);

        builder.HasOne(us => us.Skill)
            .WithMany()
            .HasForeignKey(us => us.SkillId);
    }
}