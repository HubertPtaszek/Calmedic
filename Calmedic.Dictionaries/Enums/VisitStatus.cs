using Calmedic.Resources.Shared;
using System.ComponentModel.DataAnnotations;

namespace Calmedic.Dictionaries
{
    public enum VisitStatus : int
    {
        [Display(ResourceType = typeof(EnumResource), Name = "VisitStatus_Waiting")]
        Waiting = 1,

        [Display(ResourceType = typeof(EnumResource), Name = "VisitStatus_Delayed")]
        Delayed = 2,

        [Display(ResourceType = typeof(EnumResource), Name = "VisitStatus_InProgress")]
        InProgress = 3,

        [Display(ResourceType = typeof(EnumResource), Name = "VisitStatus_Canceled")]
        Canceled = 4,

        [Display(ResourceType = typeof(EnumResource), Name = "VisitStatus_Finished")]
        Finished = 5
    }
}