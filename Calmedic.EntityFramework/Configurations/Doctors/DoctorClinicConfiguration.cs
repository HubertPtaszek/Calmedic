using Calmedic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Calmedic.EntityFramework
{
    public class DoctorClinicConfiguration : IEntityTypeConfiguration<DoctorClinic>
    {
        public DoctorClinicConfiguration()
        { }

        public void Configure(EntityTypeBuilder<DoctorClinic> builder)
        {
            builder.HasOne(x => x.Doctor)
                   .WithMany(x => x.Clinics)
                   .HasForeignKey(x => x.DoctorId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Clinic)
             .WithMany(x => x.Doctors)
             .HasForeignKey(x => x.ClinicId)
             .IsRequired()
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}