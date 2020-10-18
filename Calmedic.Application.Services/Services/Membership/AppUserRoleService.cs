using Calmedic.Data;
using Calmedic.Dictionaries;
using Calmedic.Domain;
using Calmedic.Resources.Shared;
using System.Collections.Generic;

namespace Calmedic.Application
{
    public class AppUserRoleService : ServiceBase, IAppUserRoleService
    {
        #region Dependencies
        public IAppUserRoleRepository AppUserRoleRepository { get; set; }
        public IAppUserRepository AppUserRepository { get; set; }
        public IAppRoleRepository AppRoleRepository { get; set; }
        #endregion

        public virtual List<AppRoleType> GetPermissions(int userId)
        {
            List<AppRoleType> result = new List<AppRoleType>();
            if (AppUserRoleRepository.Any(x => x.AppUserId == userId))
            {
                result = AppUserRoleRepository.GetUserRoles(userId);
            }
            return result;
        }

        public virtual void Delete(int userRoleIdId)
        {
            AppUserRoleRepository.DeleteWhere(x => x.Id == userRoleIdId);
        }

        public virtual bool IsUserInRole(int userId, int roleId)
        {
            return AppUserRoleRepository.Any(x => x.AppUserId == userId && x.AppRoleId == roleId);
        }

        public virtual void AddUserToRole(int userId, int roleId)
        {
            if (IsUserInRole(userId, roleId))
                return;
            AppUserRole crmUserRole = new AppUserRole()
            {
                AppUserId = userId,
                AppRoleId = roleId
            };
            AppUserRoleRepository.Add(crmUserRole);
        }
    }
}
