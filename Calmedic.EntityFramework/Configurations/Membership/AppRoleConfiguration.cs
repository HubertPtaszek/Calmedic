using Calmedic.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Calmedic.EntityFramework
{
    public class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
    {
        public AppRoleConfiguration()
        { }

        public void Configure(EntityTypeBuilder<AppRole> builder)
        { }
    }
}