using DevExtreme.AspNet.Data;
using Calmedic.Domain;
using System.Collections.Generic;

namespace Calmedic.Data
{
    public interface IAppUserRepository : IRepository<AppUser>
    {
        object GetUsersLookup(DataSourceLoadOptionsBase loadOptions);
        object GetUsers(DataSourceLoadOptionsBase loadOptions);
        object GetUsersforAssign(DataSourceLoadOptionsBase loadOptions, int roleId);
        object GetRoleUsers(DataSourceLoadOptionsBase loadOptions, int roleId);
        AppUser GetActive(int id);
        bool IsActive(string identityUserId);
        List<string> GetUsersEmails(List<int> usersIds);  
    }
}
