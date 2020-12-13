using Calmedic.Resources.Shared;
using System.ComponentModel.DataAnnotations;

namespace Calmedic.Dictionaries
{
    public enum ClinicType : int
    {
        [Display(ResourceType = typeof(EnumResource), Name = "ClinicType_Public")]
        Public = 1,

        [Display(ResourceType = typeof(EnumResource), Name = "ClinicType_Private")]
        Private = 2
    }
}