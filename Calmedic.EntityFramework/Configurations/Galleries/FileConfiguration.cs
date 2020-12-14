using Calmedic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Calmedic.EntityFramework
{
    public class FileConfiguration : IEntityTypeConfiguration<File>
    {
        public FileConfiguration()
        { }

        public void Configure(EntityTypeBuilder<File> builder)
        {
            builder.HasOne(x => x.Clinic)
                       .WithMany()
                       .HasForeignKey(x => x.ClinicId)
                       .IsRequired()
                       .OnDelete(DeleteBehavior.Restrict);
        }
    }
}