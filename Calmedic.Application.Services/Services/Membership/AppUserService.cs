using Calmedic.Data;
using Calmedic.Domain;
using Calmedic.Resources.Shared;
using Calmedic.Utils;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Calmedic.Application
{
    public class AppUserService : ServiceBase, IAppUserService
    {
        #region Dependencies

        public IAppUserRepository AppUserRepository { get; set; }
        public AppUserRoleService AppUserRoleService { get; set; }
        public AppUserConverter AppUserConverter { get; set; }
        public IServiceProvider ServiceProvider { get; set; }
        public IClinicRepository ClinicRepository { get; set; }

        #endregion Dependencies

        public AppUserService()
        { }

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
                Email = user.Email,
                Roles = user.UserRoles.Select(x => x.AppRole.AppRoleType).Distinct().ToList()
            };
            if (result.Roles.Contains(Dictionaries.AppRoleType.Clinic) && ClinicRepository.Any(x => x.EmailAddress == result.Email))
            {
                Clinic clinic = ClinicRepository.GetSingle(x => x.EmailAddress == result.Email);
                result.AvatarUrl = "../images/logos/" + clinic.LogoUrl;
            }
            else {
                result.AvatarUrl = user.AvatarUrl.IsNullOrEmpty() ? "../images/utils/defaultAvatar.png" : "../images/avatars/" + user.AvatarUrl;
            }
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

        public virtual object GetUsers(DataSourceLoadOptionsBase loadOptions)
        {
            return AppUserRepository.GetUsers(loadOptions);
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