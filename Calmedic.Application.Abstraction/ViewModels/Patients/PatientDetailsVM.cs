using Calmedic.Dictionaries;
using System;

namespace Calmedic.Application
{
    public class PatientDetailsVM
    {
        public PatientDetailsVM()
        {
        }

        public int Id { get; set; }
        public int PatientNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Pesel { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public SexDictionary? Sex { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string BuildingNo { get; set; }
        public string ApartmentNo { get; set; }
        public string PostalCode { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Comments { get; set; }
    }
}