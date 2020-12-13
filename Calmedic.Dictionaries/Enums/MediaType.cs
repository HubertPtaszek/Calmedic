using Calmedic.Resources.Shared;
using System.ComponentModel.DataAnnotations;

namespace Calmedic.Dictionaries
{
    public enum MediaType : int
    {
        [Display(ResourceType = typeof(EnumResource), Name = "MediaType_VisitGraphics")]
        VisitGraphics = 1,
        [Display(ResourceType = typeof(EnumResource), Name = "MediaType_Image")]
        Image = 2,
        [Display(ResourceType = typeof(EnumResource), Name = "MediaType_Video")]
        Video = 3
    }
}