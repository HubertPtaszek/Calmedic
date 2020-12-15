using Calmedic.Dictionaries;
using Calmedic.Domain;
using Calmedic.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

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
            AddSpecialization(context);
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
            if (!context.AppSettings.Any(r => r.Type == AppSettingEnum.EmailPort))
            {
                AppSetting setting = new AppSetting();
                setting.Type = AppSettingEnum.EmailPort;
                setting.Value = "587";
                context.AppSettings.Add(setting);
            }
            if (!context.AppSettings.Any(r => r.Type == AppSettingEnum.EmailServer))
            {
                AppSetting setting = new AppSetting();
                setting.Type = AppSettingEnum.EmailServer;
                setting.Value = "smtp.gmail.com";
                context.AppSettings.Add(setting);
            }
            if (!context.AppSettings.Any(r => r.Type == AppSettingEnum.EmailUserName))
            {
                AppSetting setting = new AppSetting();
                setting.Type = AppSettingEnum.EmailUserName;
                setting.Value = "kontakt.calmedic@gmail.com";
                context.AppSettings.Add(setting);
            }
            if (!context.AppSettings.Any(r => r.Type == AppSettingEnum.EmailUserPassword))
            {
                AppSetting setting = new AppSetting();
                setting.Type = AppSettingEnum.EmailUserPassword;
                setting.Value = "Rgy8v5UauYB6RBLpDDyZ";
                context.AppSettings.Add(setting);
            }
            context.SaveChanges();
        }

        #endregion AppSettings

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
            if (!context.AppRoles.Any(x => x.AppRoleType == AppRoleType.Clinic))
            {
                AppRole receptionRole = new AppRole()
                {
                    AppRoleType = AppRoleType.Clinic,
                    Name = "Przychodnia",
                    Description = "Dostęp do panelu przychodni.",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.AppRoles.Add(receptionRole);
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
            }
            context.SaveChanges();
        }

        #endregion Users

        #region Doctors

        private void AddSpecialization(MainDatabaseContext context)
        {
            SystemUser sysAdmin = context.SystemUsers.Where(x => x.Name == SystemUsers.SystemUserName).FirstOrDefault();
            if (!context.Specializations.Any())
            {
                Specialization specialization1 = new Specialization()
                {
                    Name = "Okulistyka",
                    DisplayName = "Okulista",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization1);
                Specialization specialization2 = new Specialization()
                {
                    Name = "Pediatria",
                    DisplayName = "Pediatra",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization2);
                Specialization specialization3 = new Specialization()
                {
                    Name = "Medycyna rodzinna",
                    DisplayName = "Rodzinny",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization3);
                Specialization specialization4 = new Specialization()
                {
                    Name = "Ginekologia",
                    DisplayName = "Ginekolog",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization4);
                Specialization specialization5 = new Specialization()
                {
                    Name = "Toksykologia",
                    DisplayName = "Toksykolog",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization5);
                Specialization specialization6 = new Specialization()
                {
                    Name = "Laryngologia",
                    DisplayName = "Laryngolog",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization6);
                Specialization specialization7 = new Specialization()
                {
                    Name = "Geriatria",
                    DisplayName = "Geriatra",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization7);
                Specialization specialization8 = new Specialization()
                {
                    Name = "Neurologia",
                    DisplayName = "Neurolog",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization8);
                Specialization specialization9 = new Specialization()
                {
                    Name = "Chirurgia ogólna",
                    DisplayName = "Chirurg",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization9);
                Specialization specialization10 = new Specialization()
                {
                    Name = "Dermatologia",
                    DisplayName = "Dermatolog",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization10);
                Specialization specialization11 = new Specialization()
                {
                    Name = "Dermatologia",
                    DisplayName = "Dermatolog",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization11);
                Specialization specialization12 = new Specialization()
                {
                    Name = "Alergologia",
                    DisplayName = "Alergolog",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization12);
                Specialization specialization13 = new Specialization()
                {
                    Name = "Anestezjologia",
                    DisplayName = "Anestezjolog",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization13);
                Specialization specialization14 = new Specialization()
                {
                    Name = "Diabetologia",
                    DisplayName = "Diabetolog",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization14);
                Specialization specialization15 = new Specialization()
                {
                    Name = "Pulmonologia",
                    DisplayName = "Pulmonolog",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization15);
                Specialization specialization16 = new Specialization()
                {
                    Name = "Chirurgia dziecięca",
                    DisplayName = "Chirurg",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization16);
                Specialization specialization17 = new Specialization()
                {
                    Name = "Chirurgia plastyczna",
                    DisplayName = "Chirurg",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization1);
                Specialization specialization18 = new Specialization()
                {
                    Name = "Gastroenterologia",
                    DisplayName = "Gastroenterolog",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization18);
                Specialization specialization19 = new Specialization()
                {
                    Name = "Endokrynologia",
                    DisplayName = "Endokrynolog",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization19);
                Specialization specialization20 = new Specialization()
                {
                    Name = "Kardiologia",
                    DisplayName = "Kardiolog",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization20);
                Specialization specialization21 = new Specialization()
                {
                    Name = "Medycyna pracy",
                    DisplayName = "Medycyna pracy",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization21);
                Specialization specialization22 = new Specialization()
                {
                    Name = "Onkologia",
                    DisplayName = "Onkolog",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization22);
                Specialization specialization23 = new Specialization()
                {
                    Name = "Psychiatria",
                    DisplayName = "Psychiatra",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization23);
                Specialization specialization24 = new Specialization()
                {
                    Name = "Reumatologia",
                    DisplayName = "Reumatolog",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization24);
                Specialization specialization25 = new Specialization()
                {
                    Name = "Urologia",
                    DisplayName = "Urolog",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization25);
                Specialization specialization26 = new Specialization()
                {
                    Name = "Ortodoncja",
                    DisplayName = "Ortodonta",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization26);
                Specialization specialization27 = new Specialization()
                {
                    Name = "Stomatologia",
                    DisplayName = "Stomatolog",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization27);
                Specialization specialization28 = new Specialization()
                {
                    Name = "Protetyka stomatologiczna",
                    DisplayName = "Protetyk",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization28);
            }
            context.SaveChanges();
        }

        #endregion Doctors
    }
}