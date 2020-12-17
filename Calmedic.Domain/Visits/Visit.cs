using Calmedic.Dictionaries;
using System;

namespace Calmedic.Domain
{
    public class Visit : AuditEntity
    {
        public Visit()
        { }

        public int DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }
        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Description { get; set; }
        public VisitStatus Status { get; set; }
        public int ClinicId { get; set; }
        public virtual Clinic Clinic { get; set; }
    }
}