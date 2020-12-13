using Calmedic.Resources.Shared;
using System.ComponentModel.DataAnnotations;

namespace Calmedic.Application
{
    public class ConfirmEmailVM
    {
        public string UserId { get; set; }
        public string Code { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(SharedResource), Name = "Password")]
        [RequiredShort]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(SharedResource), Name = "ConfirmedPassword")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [RequiredShort]
        public string ConfirmedPassword { get; set; }
    }
}