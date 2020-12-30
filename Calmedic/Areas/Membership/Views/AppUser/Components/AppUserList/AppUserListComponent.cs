using Calmedic.Application;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Calmedic.Core.Areas.Membership.Views.AppUser.Components.AppUserList
{
    [ViewComponent(Name = "AppUserList")]
    public class AppUserListComponent : ViewComponent
    {
        public AppUserListComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            AppUserListVM model = new AppUserListVM();
            return View("AppUserList", model);
        }
    }
}