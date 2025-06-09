using GrowSphere.Domain.Models.ProjectVacancyModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GrowSphere.Infrastructure.Configuration;

public class VacancyApplicationConfiguration : IEntityTypeConfiguration<VacancyApplication>
{
    public void Configure(EntityTypeBuilder<VacancyApplication> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Message)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>() // сохраняем enum как строку
            .IsRequired();

        builder.Property(x => x.ManagerComment)
            .HasMaxLength(1000);

        builder.HasOne<ProjectVacancy>()
            .WithMany()
            .HasForeignKey(x => x.ProjectVacancyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}