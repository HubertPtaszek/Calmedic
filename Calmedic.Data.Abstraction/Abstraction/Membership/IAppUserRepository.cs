using DevExtreme.AspNet.Data;
using Calmedic.Domain;
using System.Collections.Generic;

namespace Calmedic.Data
{
    public interface IAppUserRepository : IRepository<AppUser>
    {
        object GetUsersLookup(DataSourceLoadOptionsBase loadOptions);
        object GetUsersToList(DataSourceLoadOptionsBase loadOptions);
        AppUser GetActive(int id);
        List<string> GetUsersEmails(List<int> usersIds);  
    }
}
