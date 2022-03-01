using Calmedic.Application;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Calmedic.Core.Areas.Gallery.Views.Gallery.Components.AddFile
{
    [ViewComponent(Name = "AddFile")]
    public class AddFileComponent : ViewComponent
    {
        public AddFileComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync(GalleyAddItemVM model)
        {
            return View("AddFile", model);
        }
    }
}