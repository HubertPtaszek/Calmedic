using Calmedic.Data;
using Calmedic.Domain;
using DevExtreme.AspNet.Data;

namespace Calmedic.Application
{
    public class AppRoleService : ServiceBase, IAppRoleService
    {
        #region Dependencies
        public IAppRoleRepository AppRoleRepository { get; set; }
        public IAppUserRoleRepository AppUserRoleRepository { get; set; }
        public IAppUserRepository AppUserRepository { get; set; }
        public AppRoleConverter AppRoleConverter { get; set; }
        #endregion

        public virtual object GetRoles(DataSourceLoadOptionsBase loadOptions)
        {
            return AppRoleRepository.GetRoles(loadOptions);
        }

        public virtual AppRoleDetailsVM GetAppRoleDetailsVM(int id)
        {
            AppRole appRole = AppRoleRepository.GetSingle(x => x.Id == id);
            AppRoleDetailsVM model = AppRoleConverter.ToAppRoleDetailsVM(appRole);
            return model;
        }

        public virtual object GetUsersforAssign(DataSourceLoadOptionsBase loadOptions, int roleId)
        {
            return AppUserRepository.GetUsersforAssign(loadOptions, roleId);
        }

        public virtual object GetRoleUsers(DataSourceLoadOptionsBase loadOptions, int roleId)
        {
            return AppUserRepository.GetRoleUsers(loadOptions, roleId);
        }

        public virtual void AddUserToRole(int userId, int roleId, bool singleBinding = true)
        {
            if (singleBinding)
            {
                AppUserRoleRepository.DeleteWhere(x => x.AppUserId == userId);
            }
            AppUserRole appUserRole = new AppUserRole()
            {
                AppUserId = userId,
                AppRoleId = roleId
            };
            AppUserRoleRepository.Add(appUserRole);
        }

        public virtual void RemoveUserFromRole(int userId, int roleId)
        {
            AppUserRoleRepository.DeleteWhere(x => x.AppUserId == userId && x.AppRoleId == roleId);
        }
    }
}
