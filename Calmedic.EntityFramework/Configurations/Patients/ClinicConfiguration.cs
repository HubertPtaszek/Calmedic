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
        {
            builder.HasOne(x => x.Address)
                       .WithMany()
                       .HasForeignKey(x => x.AddressId)
                       .IsRequired()
                       .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
