using Calmedic.Resources.Shared;
using System.ComponentModel.DataAnnotations;

namespace Calmedic.Dictionaries
{
    public enum SexDictionary : int
    {
        [Display(ResourceType = typeof(EnumResource), Name = "SexDictionary_Male")]
        Male = 1,

        [Display(ResourceType = typeof(EnumResource), Name = "SexDictionary_Female")]
        Female = 2,

        [Display(ResourceType = typeof(EnumResource), Name = "SexDictionary_Different")]
        Different = 3
    }
}