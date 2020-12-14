using Calmedic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Calmedic.EntityFramework
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public PatientConfiguration()
        {
        }

        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.HasOne(x => x.Address)
                   .WithMany()
                   .HasForeignKey(x => x.AddressId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}