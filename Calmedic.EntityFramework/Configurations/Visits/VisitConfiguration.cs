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
            //builder.HasOne(x => x.Doctor)
            //    .WithMany()
            //    .HasForeignKey(x => x.CounselorId)
            //    .IsRequired()
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
