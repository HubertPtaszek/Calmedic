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
        [Display(ResourceType = typeof(SharedResource), Name = "IsActive")]
        public bool IsActive { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public List<SelectModelBinder> RoleTypes { get => EnumHelpers.GetSelectBinderList<AppRoleType>(); }
    }
}
