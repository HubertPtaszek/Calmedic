using DevExtreme.AspNet.Data;

namespace Calmedic.Application
{
    public interface IAppRoleService : IService
    {
        object GetRoles(DataSourceLoadOptionsBase loadOptions);
        AppRoleDetailsVM GetAppRoleDetailsVM(int id);
        object GetUsersforAssign(DataSourceLoadOptionsBase loadOptions, int roleId);
        object GetRoleUsers(DataSourceLoadOptionsBase loadOptions, int roleId);
        void AddUserToRole(int userId, int roleId, bool singleBinding = true);
        void RemoveUserFromRole(int userId, int roleId);
    }
}
