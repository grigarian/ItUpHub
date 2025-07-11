using GrowSphere.Domain.Models.ProjectModel;
using GrowSphere.Domain.Models.ProjectVacancyModel;
using GrowSphere.Domain.Models.Share;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GrowSphere.Infrastructure.Configuration;

public class ProjectVacancyConfiguration: IEntityTypeConfiguration<ProjectVacancy>
{
    public void Configure(EntityTypeBuilder<ProjectVacancy> builder)
    {
        builder.HasKey(x => x.Id);

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

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasMany(x => x.Skills)
            .WithOne()
            .HasForeignKey(x => x.ProjectVacancyId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(v => v.VacancyApplications)
            .WithOne(va => va.ProjectVacancy)
            .HasForeignKey(va => va.ProjectVacancyId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(p => p.ProjectId)
            .HasConversion(
                id => id.Value,
                value => ProjectId.Create(value));
        
        builder.HasOne(v => v.Project)
            .WithMany(p => p.Vacancies)
            .HasForeignKey(v => v.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
        
    }
}