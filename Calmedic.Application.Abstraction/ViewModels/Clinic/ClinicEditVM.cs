using Calmedic.Dictionaries;
using Calmedic.Resources.Shared;
using Calmedic.Utils;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Calmedic.Application
{
    public class ClinicEditVM
    {
        public ClinicEditVM()
        {
        }

        public int Id { get; set; }
        [Display(ResourceType = typeof(SharedResource), Name = "Name")]
        public string Name { get; set; }
        public Guid Guid { get; set; }
        [Display(ResourceType = typeof(SharedResource), Name = "Logo")]
        public string LogoUrl { get; set; }
        public IFormFile Logo { get; set; }
        [Display(ResourceType = typeof(SharedResource), Name = "ClinicType")]
        public ClinicType ClinicType { get; set; }
        public List<EnumModelBinder> ClinicTypes { get; set; } = EnumHelpers.GetEnumBinderList<ClinicType>();
        [Display(ResourceType = typeof(SharedResource), Name = "City")]
        public string City { get; set; }
        [Display(ResourceType = typeof(SharedResource), Name = "Street")]
        public string Street { get; set; }
        [Display(ResourceType = typeof(SharedResource), Name = "BuildingNo")]
        public string BuildingNo { get; set; }
        [Display(ResourceType = typeof(SharedResource), Name = "PostalCode")]
        public string PostalCode { get; set; }
        [Display(ResourceType = typeof(SharedResource), Name = "EmailAddress")]
        public string Email { get; set; }
        [Display(ResourceType = typeof(SharedResource), Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }
        [Display(ResourceType = typeof(SharedResource), Name = "OpenFrom")]
        public TimeSpan OpenFrom { get; set; }
        [Display(ResourceType = typeof(SharedResource), Name = "OpenTo")]
        public TimeSpan OpenTo { get; set; }
    }
}