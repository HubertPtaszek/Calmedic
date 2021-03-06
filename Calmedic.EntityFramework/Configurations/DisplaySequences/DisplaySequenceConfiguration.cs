using Calmedic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Calmedic.EntityFramework
{
    public class DisplaySequenceConfiguration : IEntityTypeConfiguration<DisplaySequence>
    {
        public DisplaySequenceConfiguration()
        { }

        public void Configure(EntityTypeBuilder<DisplaySequence> builder)
        {
            builder.HasOne(x => x.Clinic)
                       .WithMany()
                       .HasForeignKey(x => x.ClinicId)
                       .IsRequired()
                       .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.File)
                .WithMany()
                .HasForeignKey(x => x.FileId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}