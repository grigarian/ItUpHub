using GrowSphere.Domain.Models.ProjectVacancyModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GrowSphere.Domain.Models.UserModel;

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

        builder.Property(x => x.UserId)
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value));

        builder.HasOne(x => x.ProjectVacancy)
            .WithMany(v => v.VacancyApplications)
            .HasForeignKey(x => x.ProjectVacancyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany(u => u.VacancyApplications)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.UserId, x.ProjectVacancyId }).IsUnique();
    }
}