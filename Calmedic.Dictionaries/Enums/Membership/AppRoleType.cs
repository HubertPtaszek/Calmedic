using System.ComponentModel.DataAnnotations;
using Calmedic.Resources.Membership;

namespace Calmedic.Dictionaries
{
    public enum AppRoleType
    {
        [Display(ResourceType = typeof(AppRoleResource), Name = "Administrator")]
        Administrator = 0,
        [Display(ResourceType = typeof(AppRoleResource), Name = "Reception")]
        Reception = 1,
        [Display(ResourceType = typeof(AppRoleResource), Name = "Doctor")]
        Doctor = 2
    }
}
