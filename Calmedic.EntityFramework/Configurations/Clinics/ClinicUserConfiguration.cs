using Calmedic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Calmedic.EntityFramework
{
    public class ClinicUserConfiguration : IEntityTypeConfiguration<ClinicUser>
    {
        public ClinicUserConfiguration()
        { }

        public void Configure(EntityTypeBuilder<ClinicUser> builder)
        {
            builder.HasOne(x => x.User)
                    .WithMany(x => x.Clinics)
                    .HasForeignKey(x => x.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Clinic)
                    .WithMany(x => x.Users)
                    .HasForeignKey(x => x.ClinicId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}