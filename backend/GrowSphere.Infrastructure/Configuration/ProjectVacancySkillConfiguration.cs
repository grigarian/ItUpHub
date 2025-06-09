using GrowSphere.Domain.Models.ProjectVacancyModel;
using GrowSphere.Domain.Models.SkillModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GrowSphere.Infrastructure.Configuration;

public class ProjectVacancySkillConfiguration: IEntityTypeConfiguration<ProjectVacancySkill>
{
    public void Configure(EntityTypeBuilder<ProjectVacancySkill> builder)
    {
        builder.HasKey(x => new { x.ProjectVacancyId, x.SkillId });
        
        builder.Property(c => c.SkillId)
            .HasConversion(
                id => id.Value,
                value => SkillId.Create(value));

        builder.HasOne(pvs => pvs.Skill)
            .WithMany()
            .HasForeignKey(pvs => pvs.SkillId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasOne(x => x.ProjectVacancy)
            .WithMany(x => x.Skills)
            .HasForeignKey(x => x.ProjectVacancyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}