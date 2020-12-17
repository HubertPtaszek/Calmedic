using System;

namespace Calmedic.Domain
{
    public class Doctor : AuditEntity
    {
        public Doctor()
        {
        }

        public int PersonId { get; set; }
        public virtual Person Person { get; set; }
        public string Title { get; set; }
        public int SpecializationId { get; set; }
        public virtual Specialization Specialization { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime? DocumentDate { get; set; }
    }
}