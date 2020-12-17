namespace Calmedic.Domain
{
    public class ClinicUser : AuditEntity
    {
        public ClinicUser()
        {
        }

        public int UserId { get; set; }
        public virtual Person User { get; set; }
        public int ClinicId { get; set; }
        public virtual Clinic Clinic { get; set; }
    }
}