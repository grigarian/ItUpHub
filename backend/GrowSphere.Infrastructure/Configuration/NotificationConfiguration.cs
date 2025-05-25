using GrowSphere.Domain.Models.NotificationModel;
using GrowSphere.Domain.Models.UserModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GrowSphere.Infrastructure.Configuration;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("notification");
        
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Id)
            .HasConversion(
                id => id.Value,
                value => NotificationId.Create(value));
        
        builder.Property(n => n.UserId)
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value));
        
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(n => n.Message)
            .IsRequired()
            .HasMaxLength(1000)
            .HasColumnName("message");
        
        builder.Property(n => n.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");
        
        builder.Property(n => n.IsRead)
            .IsRequired()
            .HasColumnName("is_read");
    }
}