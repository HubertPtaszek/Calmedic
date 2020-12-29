using Calmedic.Dictionaries;
using Calmedic.Utils;
using System;
using System.Collections.Generic;

namespace Calmedic.Application
{
    public class PatientAddVM
    {
        public PatientAddVM()
        {
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Pesel { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public SexDictionary? Sex { get; set; }
        public List<EnumModelBinder> SexDictionaryList { get; set; } = new List<EnumModelBinder>();

        //[Display(ResourceType = typeof(SharedResource), Name = "City")]
        //[RegularExpressionSafeString]
        public string City { get; set; }

        //[Display(ResourceType = typeof(SharedResource), Name = "Street")]
        //[RegularExpressionSafeString]
        public string Street { get; set; }

        //[Display(ResourceType = typeof(SharedResource), Name = "BuildingNo")]
        //[RegularExpressionSafeString]
        public string BuildingNo { get; set; }

        //[Display(ResourceType = typeof(SharedResource), Name = "ApartmentNo")]
        //[RegularExpressionSafeString]
        public string ApartmentNo { get; set; }

        //[Display(ResourceType = typeof(SharedResource), Name = "PostalCode")]
        //[RegularExpressionPostalCode]
        public string PostalCode { get; set; }

        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Comments { get; set; }
    }
}