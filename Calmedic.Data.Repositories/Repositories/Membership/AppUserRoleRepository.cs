using Calmedic.Dictionaries;
using Calmedic.Domain;
using Calmedic.EntityFramework;
using System.Linq;
using System.Collections.Generic;

namespace Calmedic.Data
{
    public class AppUserRoleRepository : Repository<AppUserRole, MainDatabaseContext>, IAppUserRoleRepository
    {
        public AppUserRoleRepository(MainDatabaseContext context) : base(context)
        { }

        public List<AppRoleType> GetUserRoles(int userId)
        {
            return _dbset.Where(x => x.Id == userId).Select(x => x.AppRole.AppRoleType).Distinct().ToList();
        }

        public bool CheckIfIsAdmin(int userId)
        {
            return _dbset.Where(x => x.Id == userId).Any(x => x.AppRole.AppRoleType == AppRoleType.Administrator);
        }

        public bool CheckIfIsAdmin(string identityUserId)
        {
            return _dbset.Where(x => x.AppUser.AppIdentityUserId == identityUserId).Any(x => x.AppRole.AppRoleType == AppRoleType.Administrator);
        }

        public bool CheckIfIsReception(int userId)
        {
            return _dbset.Where(x => x.Id == userId).Any(x => x.AppRole.AppRoleType == AppRoleType.Reception);
        }

        public bool CheckIfIsReception(string identityUserId)
        {
            return _dbset.Where(x => x.AppUser.AppIdentityUserId == identityUserId).Any(x => x.AppRole.AppRoleType == AppRoleType.Reception);
        }

        public bool CheckIfIsDoctor(int userId)
        {
            return _dbset.Where(x => x.Id == userId).Any(x => x.AppRole.AppRoleType == AppRoleType.Doctor);
        }

        public bool CheckIfIsDoctor(string identityUserId)
        {
            return _dbset.Where(x => x.AppUser.AppIdentityUserId == identityUserId).Any(x => x.AppRole.AppRoleType == AppRoleType.Doctor);
        }
    }
}
