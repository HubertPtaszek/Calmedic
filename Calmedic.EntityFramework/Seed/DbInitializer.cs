using Microsoft.EntityFrameworkCore;
using Calmedic.Dictionaries;
using Calmedic.Domain;
using Calmedic.Utils;
using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace Calmedic.EntityFramework
{
    public class DbInitializer
    {
        private static DbInitializer _dbInitializer = null;
        private static object _lockObject = new object();

        public static void Seed(MainDatabaseContext mainDatabaseContext, UserManager<AppIdentityUser> userManager)
        {
            if (_dbInitializer != null)
            {
                return;
            }
            lock (_lockObject)
            {
                _dbInitializer = new DbInitializer();
                mainDatabaseContext.Database.Migrate();
                _dbInitializer.SeedInternal(mainDatabaseContext, userManager);
            }
        }

        protected void SeedInternal(MainDatabaseContext context, UserManager<AppIdentityUser> userManager)
        {
            AddCoreData(context);
            AddAppSeetings(context);
            AddRoles(context);
            AddAppUsers(context, userManager);
        }

        #region CoreData
        private void AddCoreData(MainDatabaseContext context)
        {
            if (!context.SystemUsers.Any())
            {
                SystemUser admin = new SystemUser()
                {
                    CreatedDate = DateTime.Now,
                    Email = SystemUsers.SystemUserEmail,
                    FirstName = SystemUsers.SystemUserName,
                    IsActive = true,
                    LastName = "",
                    Name = SystemUsers.SystemUserName
                };
                context.SystemUsers.Add(admin);
                context.SaveChanges();
                admin.CreatedById = admin.Id;
                context.Entry(admin).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
        #endregion CoreData

        #region AppSettings
        private void AddAppSeetings(MainDatabaseContext context)
        {
            context.SaveChanges();
        }
        #endregion AppSetting

        #region Roles
        private void AddRoles(MainDatabaseContext context)
        {
            SystemUser sysAdmin = context.SystemUsers.Where(x => x.Name == SystemUsers.SystemUserName).FirstOrDefault();
            if (!context.AppRoles.Any(x => x.AppRoleType == AppRoleType.Administrator))
            {
                AppRole administratorRole = new AppRole()
                {
                    AppRoleType = AppRoleType.Administrator,
                    Name = "Administrator systemu",
                    Description = "Dostęp do panelu administratora.",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.AppRoles.Add(administratorRole);
            }
            if (!context.AppRoles.Any(x => x.AppRoleType == AppRoleType.Reception))
            {
                AppRole receptionRole = new AppRole()
                {
                    AppRoleType = AppRoleType.Reception,
                    Name = "Recepcja",
                    Description = "Dostęp do panelu recepcji.",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.AppRoles.Add(receptionRole);
            }
            if (!context.AppRoles.Any(x => x.AppRoleType == AppRoleType.Doctor))
            {
                AppRole doctorRole = new AppRole()
                {
                    AppRoleType = AppRoleType.Doctor,
                    Name = "Lekarz",
                    Description = "Dostęp do panelu lekarza.",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.AppRoles.Add(doctorRole);
            }
            context.SaveChanges();
        }
        #endregion Roles

        #region Users
        private void AddAppUsers(MainDatabaseContext context, UserManager<AppIdentityUser> userManager)
        {
            SystemUser sysAdmin = context.SystemUsers.Where(x => x.Name == SystemUsers.SystemUserName).FirstOrDefault();
            if (!context.AppUsers.Any())
            {
                AppRole adminRole = context.AppRoles.FirstOrDefault(x => x.AppRoleType == AppRoleType.Administrator);
                AppRole receptionRole = context.AppRoles.FirstOrDefault(x => x.AppRoleType == AppRoleType.Reception);
                AppRole doctorRole = context.AppRoles.FirstOrDefault(x => x.AppRoleType == AppRoleType.Doctor);

                var identityUser = new AppIdentityUser() { UserName = "admin@calmedic.pl", Email = "admin@calmedic.pl", EmailConfirmed = true };
                IdentityResult identityResult = userManager.CreateAsync(identityUser, "Test.1234").Result;
                var admin = new AppUser()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Email = "admin@calmedic.pl",
                    AppIdentityUserId = identityUser.Id,
                    IsActive = true,
                    LastName = "Kowalska",
                    FirstName = "Joanna"
                };
                context.AppUsers.Add(admin);
                context.SaveChanges();

                var appUserRole = new AppUserRole()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    AppRoleId = adminRole.Id,
                    AppUserId = admin.Id,
                };
                context.AppUserRoles.Add(appUserRole);
                context.SaveChanges();

                //var normalUser = new AppIdentityUser() { UserName = "user@calmedic.pl", Email = "user@user.pl", EmailConfirmed = true };
                //IdentityResult normalUserIdentityResult = userManager.CreateAsync(normalUser, "Test.1234").Result;
                //var user = new AppUser() { CreatedById = sysAdmin.CreatedById, CreatedDate = DateTime.Now, Email = "user@user.pl", AppIdentityUserId = normalUser.Id, IsActive = true, LastName = "Użytkownik", FirstName = "Jan" };
                //context.AppUsers.Add(user);
                //context.SaveChanges();
                //var normalUserRole = new AppUserRole() { CreatedById = sysAdmin.CreatedById, CreatedDate = DateTime.Now, AppRoleId = userRole.Id, AppUserId = user.Id, };
                //context.AppUserRoles.Add(normalUserRole);
                //context.SaveChanges();
            }
        }
        #endregion Users
    }
}