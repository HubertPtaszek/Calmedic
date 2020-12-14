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

            builder.HasMany(x => x.Doctors)
                .WithOne(x => x.Clinic)
                .HasForeignKey(x => x.ClinicId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
