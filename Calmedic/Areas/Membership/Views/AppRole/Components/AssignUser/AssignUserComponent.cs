using Calmedic.Application;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Calmedic.Core.Areas.Membership.Views.AppRole.Components.AssignUser
{
    [ViewComponent(Name = "AssignUser")]
    public class AssignUserComponent : ViewComponent
    {
        public AssignUserComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync(AppRoleDetailsVM model)
        {
            return View("AssignUser", model);
        }
    }
}