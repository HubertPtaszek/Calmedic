using Calmedic.Data;
using Calmedic.Dictionaries;
using Calmedic.Domain;
using Calmedic.Resources.Shared;
using Calmedic.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Identity;

namespace Calmedic.Application
{
    public class AppUserService : ServiceBase, IAppUserService
    {
        #region Dependencies
        public IAppUserRepository AppUserRepository { get; set; }
        public AppUserRoleService AppUserRoleService { get; set; }
        public AppUserConverter AppUserConverter { get; set; }
        public IServiceProvider ServiceProvider { get; set; }
        #endregion

        public AppUserService()
        { }

        public virtual AppUserData GetFirstUser()
        {
            AppUser user = AppUserRepository.GetSingle(x => x.Email == "admin@calmedic.pl");
            if (user == null)
            {
                user = AppUserRepository.GetAll(x => x.LastName != null && x.LastName != "").OrderBy(x => x.LastName).FirstOrDefault();
            }
            AppUserData result = new AppUserData()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.Email,
                Roles = new List<AppRoleType>()
            };
            result.Roles.Add(AppRoleType.Administrator);
            return result;
        }

        public virtual AppUserData GetUserDataByAppIdentityUserId(string appIdentityUserId)
        {
            AppUser user = AppUserRepository.GetSingle(x => x.IsActive && x.AppIdentityUserId == appIdentityUserId);
            if (user == null)
                return null;
            AppUserData result = new AppUserData()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.Email,
                Roles = user.UserRoles.Select(x => x.AppRole.AppRoleType).Distinct().ToList()
            };
            return result;
        }

        public virtual AppUserDetailsVM GetAppUserDetailsVM(int userId)
        {
            UserManager<AppIdentityUser> userManager = ServiceProvider.GetService(typeof(UserManager<AppIdentityUser>)) as UserManager<AppIdentityUser>;
            AppUser crmUser = AppUserRepository.GetSingle(x => x.Id == userId);
            AppUserDetailsVM result = AppUserConverter.ToAppUserDetailsVM(crmUser);
            AppIdentityUser appUser = userManager.FindByIdAsync(crmUser.AppIdentityUserId).Result;
            result.IsEmailConfirmed = userManager.IsEmailConfirmedAsync(appUser).Result;
            return result;
        }

        public virtual AppUserEditVM GetAppUserEditVM(int userId, AppUserEditVM model = null)
        {
            if (model == null)
            {
                AppUser crmUser = AppUserRepository.GetSingle(x => x.Id == userId);
                model = AppUserConverter.ToAppUserEditVM(crmUser);

                UserManager<AppIdentityUser> userManager = ServiceProvider.GetService(typeof(UserManager<AppIdentityUser>)) as UserManager<AppIdentityUser>;
                AppIdentityUser appUser = userManager.FindByIdAsync(crmUser.AppIdentityUserId).Result;
                model.IsEmailConfirmed = userManager.IsEmailConfirmedAsync(appUser).Result;
            }
            return model;
        }
        public virtual void Edit(AppUserEditVM model, int userId)
        {
            AppUser crmUser = AppUserRepository.GetSingle(x => x.Id == userId);
            crmUser = AppUserConverter.FromAppUserEditVM(model, crmUser);
            AppUserRepository.Edit(crmUser);
        }

        public virtual object GetUsersToLookup(DataSourceLoadOptionsBase loadOptions)
        {
            return AppUserRepository.GetUsersLookup(loadOptions);
        }

        public virtual object GetUsersToList(DataSourceLoadOptionsBase loadOptions)
        {
            return AppUserRepository.GetUsersToList(loadOptions);
        }

        public virtual int Add(AppUserAddVM model)
        {
            UserManager<AppIdentityUser> userManager = ServiceProvider.GetService(typeof(UserManager<AppIdentityUser>)) as UserManager<AppIdentityUser>;

            AppIdentityUser appIdentityUser = userManager.FindByNameAsync(model.Email).Result;

            if (appIdentityUser != null)
            {
                throw new BussinesException(1000, ErrorResource.UserAlreadyAdded);
            }

            appIdentityUser = new AppIdentityUser() { UserName = model.Email, Email = model.Email };

            IdentityResult identityResult = userManager.CreateAsync(appIdentityUser).Result;
            if (!identityResult.Succeeded)
            {
                throw new BussinesException(1001, ErrorResource.UserAlreadyAdded);
            }
            AppUser user = new AppUser()
            {
                CreatedById = MainContext.PersonId,
                CreatedDate = DateTime.Now,
                IsActive = true,
                LastName = model.LastName,
                FirstName = model.FirstName,
                Email = model.Email,
                AppIdentityUserId = appIdentityUser.Id
            };
            AppUserRepository.Add(user);
            AppUserRepository.Save();
            return user.Id;
        }

        public virtual AppUserAddVM GetAppUserAddVM()
        {
            return new AppUserAddVM();
        }
    }
}
