using Calmedic.Application;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Calmedic.Core.Areas.Membership.Views.AppRole.Components.RoleUsersList
{
    [ViewComponent(Name = "RoleUsersList")]
    public class RoleUsersListComponent : ViewComponent
    {
        public RoleUsersListComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync(AppRoleDetailsVM model)
        {
            return View("RoleUsersList", model);
        }
    }
}