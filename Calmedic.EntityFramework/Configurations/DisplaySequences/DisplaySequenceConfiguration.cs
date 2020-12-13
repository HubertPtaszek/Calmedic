using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Calmedic.Domain;

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
        }
    }
}
