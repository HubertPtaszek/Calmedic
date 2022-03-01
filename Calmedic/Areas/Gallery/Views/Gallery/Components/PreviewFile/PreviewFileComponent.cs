using Calmedic.Application;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Calmedic.Core.Areas.Gallery.Views.Gallery.Components.PreviewFile
{
    [ViewComponent(Name = "PreviewFile")]
    public class PreviewFileComponent : ViewComponent
    {
        public PreviewFileComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync(GalleyItemVM model)
        {
            return View("PreviewFile", model);
        }
    }
}