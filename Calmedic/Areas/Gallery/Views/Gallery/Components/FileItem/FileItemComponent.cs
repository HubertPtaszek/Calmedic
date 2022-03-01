using Calmedic.Application;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Calmedic.Core.Areas.Gallery.Views.Gallery.Components.FileItem
{
    [ViewComponent(Name = "FileItem")]
    public class FileItemComponent : ViewComponent
    {
        public FileItemComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync(GalleyItemVM model)
        {
            return View("FileItem", model);
        }
    }
}