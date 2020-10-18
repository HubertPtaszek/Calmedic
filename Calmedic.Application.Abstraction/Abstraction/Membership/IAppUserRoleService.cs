using Calmedic.Dictionaries;
using System.Collections.Generic;

namespace Calmedic.Application
{
    public interface IAppUserRoleService : IService
    {
        List<AppRoleType> GetPermissions(int userId);
        void Delete(int userRoleId);
        bool IsUserInRole(int userId, int roleId);
        void AddUserToRole(int userId, int roleId);
    }
}
