using System;

namespace Calmedic.Domain
{
    public class Visit : AuditEntity
    {
        public Visit()
        { }

        public string Description { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int ClinicId { get; set; }
        public virtual Patient Clinic { get; set; }
    }
}
