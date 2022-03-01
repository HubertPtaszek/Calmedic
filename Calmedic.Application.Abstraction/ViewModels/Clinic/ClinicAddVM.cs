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
        public ClinicType ClinicType { get; set; }
        public List<EnumModelBinder> ClinicTypes { get; set; } = EnumHelpers.GetEnumBinderList<ClinicType>();
        public string City { get; set; }
        public string Street { get; set; }
        public string BuildingNo { get; set; }
        public string ApartmentNo { get; set; }
        public string PostalCode { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public TimeSpan OpenFrom { get; set; }
        public TimeSpan OpenTo { get; set; }
    }
}