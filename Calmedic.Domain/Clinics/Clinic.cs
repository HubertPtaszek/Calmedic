using Calmedic.Dictionaries;
using System;
using System.Collections.Generic;

namespace Calmedic.Domain
{
    public class Clinic : AuditEntity
    {
        public Clinic()
        {
        }

        public string Name { get; set; }
        public Guid Guid { get; set; }
        public string LogoUrl { get; set; }
        public ClinicType Type { get; set; }
        public int? AddressId { get; set; }
        public virtual Address Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public TimeSpan OpenFrom { get; set; }
        public TimeSpan OpenTo { get; set; }
        public virtual List<ClinicUser> Users { get; set; } = new List<ClinicUser>();
    }
}