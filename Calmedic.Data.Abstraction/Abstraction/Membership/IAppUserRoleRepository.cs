using Calmedic.Dictionaries;
using Calmedic.Domain;
using System.Collections.Generic;

namespace Calmedic.Data
{
    public interface IAppUserRoleRepository : IRepository<AppUserRole>
    {
        List<AppRoleType> GetUserRoles(int userId);
        bool CheckIfIsAdmin(string identityUserId);
        bool CheckIfIsAdmin(int userId);
        bool CheckIfIsReception(int userId);
        bool CheckIfIsReception(string identityUserId);
        bool CheckIfIsDoctor(int userId);
        bool CheckIfIsDoctor(string identityUserId);
    }
}
