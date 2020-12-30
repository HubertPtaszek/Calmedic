using System.Collections.Generic;

namespace Calmedic.Domain
{
    public abstract class Person : AuditEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string AvatarUrl { get; set; }
        public bool IsActive { get; set; }
        public bool EmailConfirmed { get; set; }
        public virtual List<ClinicUser> Clinics { get; set; } = new List<ClinicUser>();
    }
}