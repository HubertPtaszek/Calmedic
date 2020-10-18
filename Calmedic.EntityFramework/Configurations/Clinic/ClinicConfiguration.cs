using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Calmedic.Domain;

namespace Calmedic.EntityFramework
{
    public class ClinicConfiguration : IEntityTypeConfiguration<Clinic>
    {
        public ClinicConfiguration()
        { }

        public void Configure(EntityTypeBuilder<Clinic> builder)
        { }
    }
}
