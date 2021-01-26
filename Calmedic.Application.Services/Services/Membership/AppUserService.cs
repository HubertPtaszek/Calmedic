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
        public IAppUserRoleRepository AppUserRoleRepository { get; set; }
        public IAppRoleRepository AppRoleRepository { get; set; }
        public IClinicUserRepository ClinicUserRepository { get; set; }
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
                AppIdentityUserId = user.AppIdentityUserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Roles = user.UserRoles.Select(x => x.AppRole.AppRoleType).Distinct().ToList()
            };
            if (result.Roles.Contains(Dictionaries.AppRoleType.Clinic) && ClinicRepository.Any(x => x.Email == result.Email))
            {
                Clinic clinic = ClinicRepository.GetSingle(x => x.Email == result.Email);
                result.ClinicId = clinic.Id;
                result.AvatarUrl = "logos/" + clinic.LogoUrl;
            }
            else
            {
                result.AvatarUrl = user.AvatarUrl.IsNullOrEmpty() ? "utils/defaultAvatar.png" : "avatars/" + user.AvatarUrl;
            }
            return result;
        }

        public virtual AppUserDetailsVM GetAppUserDetailsVM(int userId)
        {
            UserManager<AppIdentityUser> userManager = ServiceProvider.GetService(typeof(UserManager<AppIdentityUser>)) as UserManager<AppIdentityUser>;
            AppUser user = AppUserRepository.GetSingle(x => x.Id == userId);
            AppUserDetailsVM result = AppUserConverter.ToAppUserDetailsVM(user);
            AppIdentityUser appUser = userManager.FindByIdAsync(user.AppIdentityUserId).Result;
            result.IsEmailConfirmed = userManager.IsEmailConfirmedAsync(appUser).Result;
            if (AppUserRoleRepository.Any(x => x.AppUserId == user.Id))
            {
                result.Role = AppUserRoleRepository.GetSingle(x => x.AppUserId == user.Id).AppRole.Name;
                result.RoleType = AppUserRoleRepository.GetSingle(x => x.AppUserId == user.Id).AppRole.AppRoleType;
            }
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
                IsActive = model.IsActive,
                LastName = model.LastName,
                FirstName = model.FirstName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                AppIdentityUserId = appIdentityUser.Id
            };
            AppUserRepository.Add(user);
            AppUserRepository.Save();

            AppRole role = AppRoleRepository.GetSingle(x => x.Id == model.RoleId);
            AppUserRole appUserRole = new AppUserRole()
            {
                AppRole = role,
                AppUser = user
            };
            AppUserRoleRepository.Add(appUserRole);

            if (role.AppRoleType == Dictionaries.AppRoleType.Reception)
            {
                Clinic clinic = ClinicRepository.GetSingle(x => x.Id == model.ClinicId);
                ClinicUser clinicUser = new ClinicUser()
                {
                    Clinic = clinic,
                    User = user
                };
                ClinicUserRepository.Add(clinicUser);
            }
            return user.Id;
        }

        public virtual AppUserAddVM GetAppUserAddVM()
        {
            AppUserAddVM model = new AppUserAddVM();
            model.RoleId = AppRoleRepository.GetSingle(x => x.AppRoleType == Dictionaries.AppRoleType.Reception).Id;
            model.Roles = AppRoleRepository.GetRolesToSelect();
            model.Clinics = ClinicRepository.GetClinicsToSelect();
            return model;
        }
    }
}