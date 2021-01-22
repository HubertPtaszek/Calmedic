using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Calmedic.Domain;
using Calmedic.EntityFramework;
using Calmedic.Utils;
using System.Collections.Generic;
using System.Linq;
using System;

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

        public object GetUsers(DataSourceLoadOptionsBase loadOptions)
        {
            var query = _dbset.Select(x => new
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Roles = x.UserRoles.FirstOrDefault().AppRole.Name,
                IsActive = x.IsActive,
                PhoneNumber = x.PhoneNumber
            });
            LoadResult result = DataSourceLoader.Load(query, loadOptions);
            return result;
        }

        public object GetUsersforAssign(DataSourceLoadOptionsBase loadOptions, int roleId)
        {
            var query = _dbset.Where(x => !x.UserRoles.Any(y => y.AppRoleId == roleId)).Select(x => new
            {
                Value = x.Id,
                Text = String.Join(" ", x.FirstName, x.LastName, ("<" + x.Email + ">"))
            });
            LoadResult result = DataSourceLoader.Load(query, loadOptions);
            return result;
        }

        public object GetRoleUsers(DataSourceLoadOptionsBase loadOptions, int roleId)
        {
            var query = _dbset.Where(x => x.UserRoles.Any(y => y.AppRoleId == roleId)).Select(x => new
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                IsActive = x.IsActive,
                PhoneNumber = x.PhoneNumber
            });
            LoadResult result = DataSourceLoader.Load(query, loadOptions);
            return result;
        }

        public AppUser GetActive(int id)
        {
            return _dbset.Where(x => x.IsActive).FirstOrDefault(x => x.Id == id);
        }

        public bool IsActive(string identityUserId)
        {
            return _dbset.FirstOrDefault(x => x.AppIdentityUserId == identityUserId).IsActive;
        }

        public List<string> GetUsersEmails(List<int> usersIds)
        {
            if (usersIds == null)
                return new List<string>();
            return _dbset.Where(x => usersIds.Contains(x.Id)).Select(x => x.Email).ToList();
        }
    }
}
