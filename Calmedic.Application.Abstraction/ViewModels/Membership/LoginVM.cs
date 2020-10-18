using Calmedic.Resources.Shared;
using System.ComponentModel.DataAnnotations;

namespace Calmedic.Application
{
    public class LoginVM
    {
        public LoginVM()
        { }

        [Required]
        [EmailAddress]
        [Display(ResourceType = typeof(SharedResource), Name = "EmailAddress")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(SharedResource), Name = "Password")]
        public string Password { get; set; }

        [Display(ResourceType = typeof(SharedResource), Name = "RememberMe")]
        public bool RememberMe { get; set; }
    }
}
