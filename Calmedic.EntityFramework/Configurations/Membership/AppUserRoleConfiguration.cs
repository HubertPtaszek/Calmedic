using Calmedic.Domain;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Calmedic.EntityFramework
{
    public class AppUserRoleConfiguration : IEntityTypeConfiguration<AppUserRole>
    {
        public AppUserRoleConfiguration()
        { }

        public void Configure(EntityTypeBuilder<AppUserRole> builder)
        {
            builder.HasOne(x => x.AppRole)
                .WithMany()
                .HasForeignKey(x => x.AppRoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasOne(x => x.AppUser)
                .WithMany(x => x.UserRoles)
                .HasForeignKey(x => x.AppUserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
