using Calmedic.Dictionaries;
using Calmedic.Resources.Shared;
using Calmedic.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Calmedic.Application
{
    public class ClinicDetailsVM
    {
        public ClinicDetailsVM()
        {
        }

        public int Id { get; set; }

        [Display(ResourceType = typeof(SharedResource), Name = "Name")]
        public string Name { get; set; }

        public Guid Guid { get; set; }

        [Display(ResourceType = typeof(SharedResource), Name = "Logo")]
        public string LogoUrl { get; set; }

        [Display(ResourceType = typeof(SharedResource), Name = "ClinicType")]
        public ClinicType ClinicType { get; set; }

        public List<EnumModelBinder> ClinicTypes { get; set; } = EnumHelpers.GetEnumBinderList<ClinicType>();
        public List<SelectModelBinder<int>> Specializations { get; set; }

        [Display(ResourceType = typeof(SharedResource), Name = "Address")]
        public string Address { get; set; }

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