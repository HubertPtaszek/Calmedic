using Calmedic.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Calmedic.EntityFramework
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public AppUserConfiguration()
        { }

        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasMany(x => x.UserRoles)
                .WithOne(x => x.AppUser)
                .IsRequired()
                .HasForeignKey(x => x.AppRoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.AppIdentityUserId).HasMaxLength(200);
            builder.HasIndex(x => x.AppIdentityUserId).HasName("IX_AppIdentityUserId");
        }
    }
}
