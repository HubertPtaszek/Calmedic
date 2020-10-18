using Calmedic.Resources.Shared;
using System.ComponentModel.DataAnnotations;

namespace Calmedic.Application
{
    public class ResetPasswordVM
    {   
        public string Code { get; set; }

        [RequiredShort]
        [EmailAddress]
        [Display(ResourceType = typeof(SharedResource), Name = "EmailAddress")]
        public string Email { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [RequiredShort]
        [Display(ResourceType = typeof(SharedResource), Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(SharedResource), Name = "ConfirmedPassword")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [RequiredShort]
        public string ConfirmedPassword { get; set; }
    }
}
