using Calmedic.Resources.Shared;
using System.ComponentModel.DataAnnotations;

namespace Calmedic.Application
{
    public class ForgotPasswordVM
    {
        [RequiredShort]
        [Display(ResourceType = typeof(SharedResource), Name = "EmailAddress")]
        public string Email { get; set; }        
    }
}
