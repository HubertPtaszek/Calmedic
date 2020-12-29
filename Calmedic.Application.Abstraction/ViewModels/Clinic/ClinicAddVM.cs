using Calmedic.Dictionaries;
using Calmedic.Utils;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Calmedic.Application
{
    public class ClinicAddVM
    {
        public ClinicAddVM()
        {
        }

        public string Name { get; set; }
        public IFormFile Logo { get; set; }
        public ClinicType Type { get; set; }
        public List<EnumModelBinder> ClinicTypeList { get; set; } = new List<EnumModelBinder>();
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