using Calmedic.Application;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Calmedic.Core.Areas.Membership.Views.AppRole.Components.UnassignUser
{
    [ViewComponent(Name = "UnassignUser")]
    public class UnassignUserComponent : ViewComponent
    {
        public UnassignUserComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync(AppRoleDetailsVM model)
        {
            return View("UnassignUser", model);
        }
    }
}