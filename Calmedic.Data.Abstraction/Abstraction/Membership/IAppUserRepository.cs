using DevExtreme.AspNet.Data;
using Calmedic.Domain;
using System.Collections.Generic;

namespace Calmedic.Data
{
    public interface IAppUserRepository : IRepository<AppUser>
    {
        object GetUsersLookup(DataSourceLoadOptionsBase loadOptions);
        object GetUsers(DataSourceLoadOptionsBase loadOptions);
        AppUser GetActive(int id);
        bool IsActive(string identityUserId);
        List<string> GetUsersEmails(List<int> usersIds);  
    }
}
