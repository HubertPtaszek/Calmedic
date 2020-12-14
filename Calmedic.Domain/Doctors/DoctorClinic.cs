namespace Calmedic.Domain
{
    public class DoctorClinic : AuditEntity
    {
        public DoctorClinic()
        {
        }

        public int DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }
        public int ClinicId { get; set; }
        public virtual Clinic Clinic { get; set; }
    }
}