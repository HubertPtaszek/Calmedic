using Calmedic.Dictionaries;
using Calmedic.Resources.Shared;
using Calmedic.Utils;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Calmedic.Application
{
    public class AppUserAddVM
    {
        [RequiredShort]
        [Display(ResourceType = typeof(SharedResource), Name = "LastName")]
        public string LastName { get; set; }
        [RequiredShort]
        [Display(ResourceType = typeof(SharedResource), Name = "FirstName")]
        public string FirstName { get; set; }
        [RequiredShort]
        [Display(ResourceType = typeof(SharedResource), Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }
        [Display(ResourceType = typeof(SharedResource), Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }
        [Display(ResourceType = typeof(SharedResource), Name = "IsActive")]
        public bool IsActive { get; set; } = false;
        [RequiredShort]
        [Display(ResourceType = typeof(SharedResource), Name = "Role")]
        public int RoleId { get; set; }
        public List<SelectModelBinder<int>> Roles { get; set; }
        [Display(ResourceType = typeof(SharedResource), Name = "Clinic")]
        [RequiredShort]
        public int ClinicId { get; set; }
        public List<SelectModelBinder<int>> Clinics { get; set; }
    }
}
