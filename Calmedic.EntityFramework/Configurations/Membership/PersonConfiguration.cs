using Calmedic.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Calmedic.EntityFramework
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public PersonConfiguration()
        { }

        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasOne(x => x.CreatedBy)
                .WithMany()
                .HasForeignKey(x => x.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ModifiedBy)
                .WithMany()
                .HasForeignKey(x => x.ModifiedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
