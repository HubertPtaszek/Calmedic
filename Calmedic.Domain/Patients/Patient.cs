using Calmedic.Dictionaries;
using System;

namespace Calmedic.Domain
{
    public class Patient : AuditEntity
    {
        public Patient()
        {
        }

        public int PatientNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Pesel { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public SexDictionary? Sex { get; set; }
        public int? AddressId { get; set; }
        public virtual Address Address { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Comments { get; set; }
    }
}