using Calmedic.Resources.Shared;
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
    }
}
