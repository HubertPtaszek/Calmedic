using Calmedic.Dictionaries;
using System;

namespace Calmedic.Application
{
    public class ClinicDetailsVM
    {
        public ClinicDetailsVM()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public Guid Guid { get; set; }
        public string LogoUrl { get; set; }
        public ClinicType Type { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string BuildingNo { get; set; }
        public string ApartmentNo { get; set; }
        public string PostalCode { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public TimeSpan OpenFrom { get; set; }
        public TimeSpan OpenTo { get; set; }
    }
}