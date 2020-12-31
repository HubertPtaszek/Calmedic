using Calmedic.Application;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Calmedic.Core.Areas.Membership.Views.AppUser.Components.AppRoleList
{
    [ViewComponent(Name = "AppRoleList")]
    public class AppRoleListComponent : ViewComponent
    {
        public AppRoleListComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            AppRoleListVM model = new AppRoleListVM();
            return View("AppRoleList", model);
        }
    }
}