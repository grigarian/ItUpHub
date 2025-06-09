using GrowSphere.Domain.Models.UserModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GrowSphere.Infrastructure.Configuration
{
    public class UserConfiguration :IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("user");
        
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id)
                .HasConversion(
                    id => id.Value,
                    value => UserId.Create(value));

            builder.ComplexProperty(i => i.Name, t =>
            {
                t.Property(p => p.Value)
                    .IsRequired()
                    .HasMaxLength(User.USERNAME_MAX_LENGTH)
                    .HasColumnName("username");
            });

            builder.ComplexProperty(u => u.Email, t =>
            {
                t.Property(p => p.Value)
                    .IsRequired()
                    .HasColumnName("email");
            });
            
            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("password");

            builder.ComplexProperty(u => u.ProfilePicture, t =>
            {
                t.Property(p => p.Path)
                    .IsRequired(false)
                    .HasColumnName("path");
                t.Property(p => p.MimeType)
                    .IsRequired(false)
                    .HasColumnName("mime_type");
            });
            
            builder.ComplexProperty(u => u.Bio, t =>
            {
                t.Property(p => p.Value)
                    .IsRequired(false)
                    .HasColumnName("bio");
            });

            builder.Property(u => u.IsAdmin)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName("is_admin");

            builder.HasMany(u => u.Skills)
                .WithOne(us => us.User)
                .HasForeignKey(us => us.UserId)
                .IsRequired(false);

            builder.ComplexProperty(u => u.UserStatus, us =>
            {
                us.Property(p => p.Value)
                    .IsRequired(false)
                    .HasColumnName("status");
            });
            
            builder.Property(u => u.CreatedDate)
                .IsRequired()
                .HasColumnName("created_at");

            builder.HasMany(u => u.Projects)
                .WithOne(up => up.User)
                .HasForeignKey(up => up.UserId)
                .IsRequired(false);
        }
    }
}
