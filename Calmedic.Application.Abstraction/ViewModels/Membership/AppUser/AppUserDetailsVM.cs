using Calmedic.Dictionaries;
using Calmedic.Resources.Shared;
using Calmedic.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calmedic.Application
{
    public class AppUserDetailsVM
    {
        public int Id { get; set; }
        public string IdentityUserId { get; set; }

        [Display(ResourceType = typeof(SharedResource), Name = "LastName")]
        public string LastName { get; set; }
        [Display(ResourceType = typeof(SharedResource), Name = "FirstName")]
        public string FirstName { get; set; }
        [Display(ResourceType = typeof(SharedResource), Name = "Email")]
        public string Email { get; set; }
        [Display(ResourceType = typeof(SharedResource), Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }
        [Display(ResourceType = typeof(SharedResource), Name = "IsActive")]
        public bool IsActive { get; set; }
        [Display(ResourceType = typeof(SharedResource), Name = "IsEmailConfirmed")]
        public bool IsEmailConfirmed { get; set; }
        [Display(ResourceType = typeof(SharedResource), Name = "Role")]
        public string Role { get; set; }
        public AppRoleType RoleType { get; set; }
        [Display(ResourceType = typeof(SharedResource), Name = "Clinic")]
        public string Clinic { get; set; }
    }
}
