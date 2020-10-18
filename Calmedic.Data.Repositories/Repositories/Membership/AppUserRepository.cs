using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Calmedic.Domain;
using Calmedic.EntityFramework;
using Calmedic.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Calmedic.Data
{
    public class AppUserRepository : Repository<AppUser, MainDatabaseContext>, IAppUserRepository
    {
        public AppUserRepository(MainDatabaseContext context) : base(context)
        { }

        public object GetUsersLookup(DataSourceLoadOptionsBase loadOptions)
        {
            IQueryable<SelectModelBinder<int>> query = _dbset.Select(x => new SelectModelBinder<int>() { Value = x.Id, Text = x.LastName + " " + x.FirstName });
            LoadResult result = DataSourceLoader.Load(query, loadOptions);
            return result.data;
        }

        public object GetUsersToList(DataSourceLoadOptionsBase loadOptions)
        {
            var internalQuery = _dbset.Select(x => new
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                IsActive = x.IsActive,
                Email = x.Email,
                Roles = x.UserRoles.Select(y => y.AppRole.Name).ToList()
            });
            var query = internalQuery.ToList().Select(x => new
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                IsActive = x.IsActive,
                Email = x.Email,
                Roles = string.Join(", ", x.Roles)
            });
            LoadResult result = DataSourceLoader.Load(query, loadOptions);
            return result;
        }

        public AppUser GetActive(int id)
        {
            return _dbset.Where(x => x.IsActive).FirstOrDefault(x => x.Id == id);
        }

        public List<string> GetUsersEmails(List<int> usersIds)
        {
            if (usersIds == null)
                return new List<string>();
            return _dbset.Where(x => usersIds.Contains(x.Id)).Select(x => x.Email).ToList();
        }
    }
}
