using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Calmedic.Domain;

namespace Calmedic.EntityFramework
{
    public class VisitConfiguration : IEntityTypeConfiguration<Visit>
    {
        public VisitConfiguration()
        { }

        public void Configure(EntityTypeBuilder<Visit> builder)
        {
            builder.HasOne(x => x.Doctor)
                .WithMany()
                .HasForeignKey(x => x.DoctorId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Patient)
                .WithMany()
                .HasForeignKey(x => x.PatientId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Clinic)
               .WithMany()
               .HasForeignKey(x => x.ClinicId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
