using Calmedic.Dictionaries;
using Calmedic.Resources.Shared;
using Calmedic.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Calmedic.Application
{
    public class PatientDetailsVM
    {
        public PatientDetailsVM()
        {
        }

        public int Id { get; set; }
        [Display(ResourceType = typeof(SharedResource), Name = "PatientNumber")]
        public int PatientNumber { get; set; }
        [Display(ResourceType = typeof(SharedResource), Name = "FirstName")]
        public string FirstName { get; set; }
        [Display(ResourceType = typeof(SharedResource), Name = "LastName")]
        public string LastName { get; set; }
        [Display(ResourceType = typeof(SharedResource), Name = "Pesel")]
        public string Pesel { get; set; }
        [Display(ResourceType = typeof(SharedResource), Name = "DateOfBirth")]
        public DateTime DateOfBirth { get; set; }
        [Display(ResourceType = typeof(SharedResource), Name = "Sex")]
        public SexDictionary? Sex { get; set; }
        public List<EnumModelBinder> SexDictionary { get; set; } = EnumHelpers.GetEnumBinderList<SexDictionary>();
        [Display(ResourceType = typeof(SharedResource), Name = "Address")]
        public string Address { get; set; }
        [Display(ResourceType = typeof(SharedResource), Name = "EmailAddress")]
        public string EmailAddress { get; set; }
        [Display(ResourceType = typeof(SharedResource), Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }
        [Display(ResourceType = typeof(SharedResource), Name = "Comments")]
        public string Comments { get; set; }
    }
}