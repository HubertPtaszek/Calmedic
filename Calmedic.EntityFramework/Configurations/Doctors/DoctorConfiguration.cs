using Calmedic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Calmedic.EntityFramework
{
    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public DoctorConfiguration()
        { }

        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.HasOne(x => x.Specialization)
                       .WithMany()
                       .HasForeignKey(x => x.SpecializationId)
                       .IsRequired()
                       .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Person)
                       .WithMany()
                       .HasForeignKey(x => x.PersonId)
                       .IsRequired();

            builder.HasMany(x => x.Clinics)
                .WithOne(x => x.Doctor)
                .HasForeignKey(x => x.DoctorId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}