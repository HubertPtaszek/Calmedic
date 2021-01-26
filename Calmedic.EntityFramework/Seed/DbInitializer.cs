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

            AddTestData(context, userManager);
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

                var identityUser = new AppIdentityUser() { UserName = "admin@calmedic.pl", Email = "admin@calmedic.pl", EmailConfirmed = true };
                IdentityResult identityResult = userManager.CreateAsync(identityUser, "Test.1234").Result;
                var admin = new AppUser()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Email = "admin@calmedic.pl",
                    PhoneNumber = "+48 123 456 789",
                    AppIdentityUserId = identityUser.Id,
                    IsActive = true,
                    LastName = "",
                    FirstName = "Administrator"
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
                    Name = "Alergologia",
                    DisplayName = "Alergolog",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization11);
                Specialization specialization12 = new Specialization()
                {
                    Name = "Anestezjologia",
                    DisplayName = "Anestezjolog",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization12);
                Specialization specialization13 = new Specialization()
                {
                    Name = "Diabetologia",
                    DisplayName = "Diabetolog",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization13);
                Specialization specialization14 = new Specialization()
                {
                    Name = "Pulmonologia",
                    DisplayName = "Pulmonolog",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization14);
                Specialization specialization15 = new Specialization()
                {
                    Name = "Chirurgia dziecięca",
                    DisplayName = "Chirurg",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization15);
                Specialization specialization16 = new Specialization()
                {
                    Name = "Chirurgia plastyczna",
                    DisplayName = "Chirurg",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization16);
                Specialization specialization17 = new Specialization()
                {
                    Name = "Gastroenterologia",
                    DisplayName = "Gastroenterolog",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization17);
                Specialization specialization18 = new Specialization()
                {
                    Name = "Endokrynologia",
                    DisplayName = "Endokrynolog",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization18);
                Specialization specialization19 = new Specialization()
                {
                    Name = "Kardiologia",
                    DisplayName = "Kardiolog",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization19);
                Specialization specialization20 = new Specialization()
                {
                    Name = "Medycyna pracy",
                    DisplayName = "Medycyna pracy",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization20);
                Specialization specialization21 = new Specialization()
                {
                    Name = "Onkologia",
                    DisplayName = "Onkolog",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization21);
                Specialization specialization22 = new Specialization()
                {
                    Name = "Psychiatria",
                    DisplayName = "Psychiatra",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization22);
                Specialization specialization23 = new Specialization()
                {
                    Name = "Reumatologia",
                    DisplayName = "Reumatolog",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization23);
                Specialization specialization24 = new Specialization()
                {
                    Name = "Urologia",
                    DisplayName = "Urolog",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization24);
                Specialization specialization25 = new Specialization()
                {
                    Name = "Ortodoncja",
                    DisplayName = "Ortodonta",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization25);
                Specialization specialization26 = new Specialization()
                {
                    Name = "Stomatologia",
                    DisplayName = "Stomatolog",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization26);
                Specialization specialization27 = new Specialization()
                {
                    Name = "Protetyka stomatologiczna",
                    DisplayName = "Protetyk",
                    Description = "",
                    CreatedById = sysAdmin.Id,
                    CreatedDate = DateTime.Now
                };
                context.Specializations.Add(specialization27);
            }
            context.SaveChanges();
        }

        #endregion Doctors

        #region Test

        private void AddTestData(MainDatabaseContext context, UserManager<AppIdentityUser> userManager)
        {
            SystemUser sysAdmin = context.SystemUsers.Where(x => x.Name == SystemUsers.SystemUserName).FirstOrDefault();
            if (!context.Clinics.Any() && !context.Addresses.Any())
            {
                AppRole clinicRole = context.AppRoles.FirstOrDefault(x => x.AppRoleType == AppRoleType.Clinic);
                AppRole doctorRole = context.AppRoles.FirstOrDefault(x => x.AppRoleType == AppRoleType.Doctor);

                AppIdentityUser identityUser = new AppIdentityUser() { UserName = "melisa@calmedic.pl", Email = "melisa@calmedic.pl", EmailConfirmed = true };
                IdentityResult identityResult = userManager.CreateAsync(identityUser, "Test.1234").Result;
                AppUser user = new AppUser()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Email = "melisa@calmedic.pl",
                    PhoneNumber = "+48 123 456 789",
                    AppIdentityUserId = identityUser.Id,
                    IsActive = true,
                    LastName = "",
                    FirstName = "Melisa"
                };
                context.AppUsers.Add(user);
                context.SaveChanges();

                AppUserRole appUserRole = new AppUserRole()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    AppRoleId = clinicRole.Id,
                    AppUserId = user.Id,
                };
                context.AppUserRoles.Add(appUserRole);

                Address addressClinic = new Address()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    ApartmentNo = "",
                    BuildingNo = "17a",
                    City = "Kielce",
                    Street = "Wymyślna",
                    PostalCode = "25-500",
                    FullAdress = "Wymyślna 17a, 25-500 Kielce"
                };
                context.Addresses.Add(addressClinic);

                Clinic clinic = new Clinic()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Name = "Melisa",
                    Guid = new Guid(),
                    LogoUrl = "melisa.png",
                    Type = ClinicType.Private,
                    Address = addressClinic,
                    Email = "melisa@calmedic.pl",
                    PhoneNumber = "+48 123 456 789",
                    OpenFrom = new TimeSpan(6, 0, 0),
                    OpenTo = new TimeSpan(18, 0, 0)
                };
                context.Clinics.Add(clinic);

                ClinicUser userClinic = new ClinicUser()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Clinic = clinic,
                    User = user
                };
                context.ClinicUsers.Add(userClinic);

                Address address1 = new Address()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    ApartmentNo = "12",
                    BuildingNo = "7",
                    City = "Kielce",
                    Street = "Wymyślna",
                    PostalCode = "25-500",
                    FullAdress = "Wymyślna 7/12, 25-500 Kielce"
                };
                context.Addresses.Add(address1);
                Patient patient1 = new Patient()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now.AddMinutes(-1),
                    Address = address1,
                    FirstName = "Wojciech",
                    LastName = "Celjor",
                    Comments = "Alergik.",
                    DateOfBirth = new DateTime(1957, 11, 12),
                    Pesel = "57111281976",
                    PatientNumber = 10001,
                    Sex = SexDictionary.Male,
                    EmailAddress = "wojtek.c@o2.pl",
                    PhoneNumber = "+48 123 456 789"
                };
                context.Patients.Add(patient1);

                Address address2 = new Address()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    ApartmentNo = "",
                    BuildingNo = "2",
                    City = "Kielce",
                    Street = "Wymyślna",
                    PostalCode = "25-500",
                    FullAdress = "Wymyślna 2, 25-500 Kielce"
                };
                context.Addresses.Add(address2);
                Patient patient2 = new Patient()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now.AddMinutes(-2),
                    Address = address1,
                    FirstName = "Marian",
                    LastName = "Murarz",
                    Comments = "",
                    DateOfBirth = new DateTime(1958, 7, 3),
                    Pesel = "58070389859",
                    PatientNumber = 10002,
                    Sex = SexDictionary.Male,
                    EmailAddress = "marianmurarz@wp.pl",
                    PhoneNumber = "+48 123 456 789"
                };
                context.Patients.Add(patient2);

                Address address3 = new Address()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    ApartmentNo = "4",
                    BuildingNo = "4",
                    City = "Kielce",
                    Street = "Wymyślna",
                    PostalCode = "25-500",
                    FullAdress = "Wymyślna 4/4, 25-500 Kielce"
                };
                context.Addresses.Add(address3);
                Patient patient3 = new Patient()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now.AddMinutes(-3),
                    Address = address3,
                    FirstName = "Karol",
                    LastName = "Kowalski",
                    Comments = "",
                    DateOfBirth = new DateTime(1996, 4, 28),
                    Pesel = "96042824612",
                    PatientNumber = 10003,
                    Sex = SexDictionary.Male,
                    EmailAddress = "karolkarol@wp.pl",
                    PhoneNumber = "+48 123 456 789"
                };
                context.Patients.Add(patient3);

                Address address4 = new Address()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    ApartmentNo = "",
                    BuildingNo = "6a",
                    City = "Kielce",
                    Street = "Wymyślna",
                    PostalCode = "25-500",
                    FullAdress = "Wymyślna 6a, 25-500 Kielce"
                };
                context.Addresses.Add(address4);
                Patient patient4 = new Patient()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now.AddMinutes(-4),
                    Address = address4,
                    FirstName = "Jarosław",
                    LastName = "Kowalski",
                    Comments = "Demencja starcza.",
                    DateOfBirth = new DateTime(1950, 3, 9),
                    Pesel = "50030951779",
                    PatientNumber = 10004,
                    Sex = SexDictionary.Male,
                    EmailAddress = "",
                    PhoneNumber = "+48 123 456 789"
                };
                context.Patients.Add(patient4);

                Address address5 = new Address()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    ApartmentNo = "",
                    BuildingNo = "6c",
                    City = "Kielce",
                    Street = "Wymyślna",
                    PostalCode = "25-500",
                    FullAdress = "Wymyślna 6c, 25-500 Kielce"
                };
                context.Addresses.Add(address5);
                Patient patient5 = new Patient()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now.AddMinutes(-5),
                    Address = address5,
                    FirstName = "Tomasz",
                    LastName = "Marcinek",
                    Comments = "",
                    DateOfBirth = new DateTime(1987, 7, 16),
                    Pesel = "87071696637",
                    PatientNumber = 10005,
                    Sex = SexDictionary.Male,
                    EmailAddress = "tomekm@gmail.com",
                    PhoneNumber = "+48 123 456 789"
                };
                context.Patients.Add(patient5);

                Address address6 = new Address()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    ApartmentNo = "12",
                    BuildingNo = "98",
                    City = "Kielce",
                    Street = "Wymyślna",
                    PostalCode = "25-500",
                    FullAdress = "Wymyślna 98/12, 25-500 Kielce"
                };
                context.Addresses.Add(address6);
                Patient patient6 = new Patient()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now.AddMinutes(-6),
                    Address = address6,
                    FirstName = "Tomasz",
                    LastName = "Kaleta",
                    Comments = "",
                    DateOfBirth = new DateTime(1983, 6, 7),
                    Pesel = "83060728491",
                    PatientNumber = 10006,
                    Sex = SexDictionary.Male,
                    EmailAddress = "",
                    PhoneNumber = "+48 123 456 789"
                };
                context.Patients.Add(patient6);

                Address address7 = new Address()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    ApartmentNo = "",
                    BuildingNo = "90",
                    City = "Kielce",
                    Street = "Wymyślna",
                    PostalCode = "25-500",
                    FullAdress = "Wymyślna 90, 25-500 Kielce"
                };
                context.Addresses.Add(address7);
                Patient patient7 = new Patient()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now.AddMinutes(-7),
                    Address = address7,
                    FirstName = "Janusz",
                    LastName = "Nowak",
                    Comments = "",
                    DateOfBirth = new DateTime(1949, 9, 27),
                    Pesel = "49092715294",
                    PatientNumber = 10007,
                    Sex = SexDictionary.Male,
                    EmailAddress = "",
                    PhoneNumber = "+48 123 456 789"
                };
                context.Patients.Add(patient7);

                Address address8 = new Address()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    ApartmentNo = "14",
                    BuildingNo = "9",
                    City = "Kielce",
                    Street = "Wymyślna",
                    PostalCode = "25-500",
                    FullAdress = "Wymyślna 9/14, 25-500 Kielce"
                };
                context.Addresses.Add(address8);
                Patient patient8 = new Patient()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now.AddMinutes(-8),
                    Address = address8,
                    FirstName = "Paweł",
                    LastName = "Nowak",
                    Comments = "",
                    DateOfBirth = new DateTime(1997, 7, 13),
                    Pesel = "97071354914",
                    PatientNumber = 10008,
                    Sex = SexDictionary.Male,
                    EmailAddress = "pawełnowak@gmail.com",
                    PhoneNumber = "+48 123 456 789"
                };
                context.Patients.Add(patient8);

                Address address9 = new Address()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    ApartmentNo = "28",
                    BuildingNo = "9",
                    City = "Kielce",
                    Street = "Wymyślna",
                    PostalCode = "25-500",
                    FullAdress = "Wymyślna 9/28, 25-500 Kielce"
                };
                context.Addresses.Add(address9);
                Patient patient9 = new Patient()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now.AddMinutes(-9),
                    Address = address9,
                    FirstName = "Maja",
                    LastName = "Bogusz",
                    Comments = "",
                    DateOfBirth = new DateTime(1992, 6, 25),
                    Pesel = "92062561843",
                    PatientNumber = 10009,
                    Sex = SexDictionary.Female,
                    EmailAddress = "maja92@gmail.com",
                    PhoneNumber = "+48 123 456 789"
                };
                context.Patients.Add(patient9);

                Address address10 = new Address()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    ApartmentNo = "4",
                    BuildingNo = "9",
                    City = "Kielce",
                    Street = "Wymyślna",
                    PostalCode = "25-500",
                    FullAdress = "Wymyślna 9/4, 25-500 Kielce"
                };
                context.Addresses.Add(address10);
                Patient patient10 = new Patient()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now.AddMinutes(-10),
                    Address = address10,
                    FirstName = "Maria",
                    LastName = "Tokar",
                    Comments = "Genetycznie wrodzone choroby układu odpornościowego.",
                    DateOfBirth = new DateTime(1969, 9, 23),
                    Pesel = "69092322386",
                    PatientNumber = 10010,
                    Sex = SexDictionary.Female,
                    EmailAddress = "",
                    PhoneNumber = "+48 123 456 789"
                };
                context.Patients.Add(patient10);

                Address address11 = new Address()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    ApartmentNo = "",
                    BuildingNo = "47",
                    City = "Kielce",
                    Street = "Wymyślna",
                    PostalCode = "25-500",
                    FullAdress = "Wymyślna 47, 25-500 Kielce"
                };
                context.Addresses.Add(address11);
                Patient patient11 = new Patient()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now.AddMinutes(-11),
                    Address = address11,
                    FirstName = "Maria",
                    LastName = "Tokar",
                    Comments = "Genetycznie wrodzone choroby układu odpornościowego.",
                    DateOfBirth = new DateTime(1951, 9, 22),
                    Pesel = "51092271548",
                    PatientNumber = 10011,
                    Sex = SexDictionary.Female,
                    EmailAddress = "",
                    PhoneNumber = "+48 123 456 789"
                };
                context.Patients.Add(patient11);

                Address address12 = new Address()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    ApartmentNo = "",
                    BuildingNo = "74",
                    City = "Kielce",
                    Street = "Wymyślna",
                    PostalCode = "25-500",
                    FullAdress = "Wymyślna 74, 25-500 Kielce"
                };
                context.Addresses.Add(address12);
                Patient patient12 = new Patient()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now.AddMinutes(-12),
                    Address = address12,
                    FirstName = "Teresa",
                    LastName = "Krzak",
                    Comments = "",
                    DateOfBirth = new DateTime(1948, 12, 27),
                    Pesel = "48122799369",
                    PatientNumber = 10012,
                    Sex = SexDictionary.Female,
                    EmailAddress = "",
                    PhoneNumber = "+48 123 456 789"
                };
                context.Patients.Add(patient12);

                Address address13 = new Address()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    ApartmentNo = "7",
                    BuildingNo = "77",
                    City = "Kielce",
                    Street = "Wymyślna",
                    PostalCode = "25-500",
                    FullAdress = "Wymyślna 77/7, 25-500 Kielce"
                };
                context.Addresses.Add(address13);
                Patient patient13 = new Patient()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now.AddMinutes(-13),
                    Address = address13,
                    FirstName = "Mariola",
                    LastName = "Worek",
                    Comments = "",
                    DateOfBirth = new DateTime(1974, 5, 16),
                    Pesel = "74051615548",
                    PatientNumber = 10013,
                    Sex = SexDictionary.Female,
                    EmailAddress = "mariolaw@onet.pl",
                    PhoneNumber = "+48 123 456 789"
                };
                context.Patients.Add(patient13);

                Address address14 = new Address()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    ApartmentNo = "45",
                    BuildingNo = "77",
                    City = "Kielce",
                    Street = "Wymyślna",
                    PostalCode = "25-500",
                    FullAdress = "Wymyślna 77/45, 25-500 Kielce"
                };
                context.Addresses.Add(address14);
                Patient patient14 = new Patient()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now.AddMinutes(-14),
                    Address = address14,
                    FirstName = "Aleksandra",
                    LastName = "Zawadzka",
                    Comments = "",
                    DateOfBirth = new DateTime(1984, 8, 7),
                    Pesel = "84080741369",
                    PatientNumber = 10014,
                    Sex = SexDictionary.Female,
                    EmailAddress = "olaz84@onet.pl",
                    PhoneNumber = "+48 123 456 789"
                };
                context.Patients.Add(patient14);

                Address address15 = new Address()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    ApartmentNo = "45",
                    BuildingNo = "7",
                    City = "Kielce",
                    Street = "Wymyślna",
                    PostalCode = "25-500",
                    FullAdress = "Wymyślna 7/45, 25-500 Kielce"
                };
                context.Addresses.Add(address15);
                Patient patient15 = new Patient()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now.AddMinutes(-15),
                    Address = address15,
                    FirstName = "Karolina",
                    LastName = "Kowalczyk",
                    Comments = "Osoba po przeszczepie nerki.",
                    DateOfBirth = new DateTime(1967, 11, 9),
                    Pesel = "67110911127",
                    PatientNumber = 10015,
                    Sex = SexDictionary.Female,
                    EmailAddress = "",
                    PhoneNumber = "+48 123 456 789"
                };
                context.Patients.Add(patient15);

                Address address16 = new Address()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    ApartmentNo = "",
                    BuildingNo = "65",
                    City = "Kielce",
                    Street = "Wymyślna",
                    PostalCode = "25-500",
                    FullAdress = "Wymyślna 65, 25-500 Kielce"
                };
                context.Addresses.Add(address16);
                Patient patient16 = new Patient()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now.AddMinutes(-16),
                    Address = address16,
                    FirstName = "Stanisława",
                    LastName = "Kowalczyk",
                    Comments = "",
                    DateOfBirth = new DateTime(1948, 2, 20),
                    Pesel = "48022038368",
                    PatientNumber = 10016,
                    Sex = SexDictionary.Female,
                    EmailAddress = "",
                    PhoneNumber = "+48 123 456 789"
                };
                context.Patients.Add(patient16);

                Address address17 = new Address()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    ApartmentNo = "",
                    BuildingNo = "65",
                    City = "Kielce",
                    Street = "Wymyślna",
                    PostalCode = "25-500",
                    FullAdress = "Wymyślna 65, 25-500 Kielce"
                };
                context.Addresses.Add(address17);
                Patient patient17 = new Patient()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now.AddMinutes(-17),
                    Address = address17,
                    FirstName = "Nikola",
                    LastName = "Krawiec",
                    Comments = "",
                    DateOfBirth = new DateTime(2001, 5, 5),
                    Pesel = "01250557326",
                    PatientNumber = 10017,
                    Sex = SexDictionary.Female,
                    EmailAddress = "nikiniki@o2.pl",
                    PhoneNumber = "+48 123 456 789"
                };
                context.Patients.Add(patient17);


                Specialization specialization1 = context.Specializations.Where(x => x.Name == "Medycyna rodzinna").FirstOrDefault();
                Specialization specialization2 = context.Specializations.Where(x => x.Name == "Stomatologia").FirstOrDefault();
                Specialization specialization3 = context.Specializations.Where(x => x.Name == "Psychiatria").FirstOrDefault();
                Specialization specialization4 = context.Specializations.Where(x => x.Name == "Kardiologia").FirstOrDefault();
                Specialization specialization5 = context.Specializations.Where(x => x.Name == "Dermatologia").FirstOrDefault();

                AppIdentityUser identityUser1 = new AppIdentityUser() { UserName = "marekprylinski@calmedic.pl", Email = "marekprylinski@calmedic.pl", EmailConfirmed = true };
                IdentityResult identityResult1 = userManager.CreateAsync(identityUser1, "Test.1234").Result;
                AppUser user1 = new AppUser()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    AvatarUrl = "1.png",
                    Email = "marekprylinski@calmedic.pl",
                    PhoneNumber = "+48 123 456 789",
                    AppIdentityUserId = identityUser1.Id,
                    FirstName = "Marek",
                    LastName = "Pryliński",
                    IsActive = true
                };
                Doctor doctor1 = new Doctor()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    DocumentDate = new DateTime(2015, 6, 1),
                    DocumentNumber = "5425740",
                    Person = user1,
                    Specialization = specialization1,
                    Title = "Dr Hab. N. Med."
                };
                context.Doctors.Add(doctor1);

                AppIdentityUser identityUser2 = new AppIdentityUser() { UserName = "jakubpanek@calmedic.pl", Email = "jakubpanek@calmedic.pl", EmailConfirmed = true };
                IdentityResult identityResult2 = userManager.CreateAsync(identityUser2, "Test.1234").Result;
                AppUser user2 = new AppUser()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    AvatarUrl = "2.png",
                    Email = "jakubpanek@calmedic.pl",
                    PhoneNumber = "+48 123 456 789",
                    AppIdentityUserId = identityUser2.Id,
                    FirstName = "Jakub",
                    LastName = "Panek",
                    IsActive = true
                };
                Doctor doctor2 = new Doctor()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    DocumentDate = new DateTime(2015, 6, 1),
                    DocumentNumber = "5429840",
                    Person = user2,
                    Specialization = specialization2,
                    Title = "Dr N. Med."
                };
                context.Doctors.Add(doctor2);

                AppIdentityUser identityUser3 = new AppIdentityUser() { UserName = "lukaszbiernat@calmedic.pl", Email = "lukaszbiernat@calmedic.pl", EmailConfirmed = true };
                IdentityResult identityResult3 = userManager.CreateAsync(identityUser3, "Test.1234").Result;
                AppUser user3 = new AppUser()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    AvatarUrl = "3.png",
                    Email = "lukaszbiernat@calmedic.pl",
                    PhoneNumber = "+48 123 456 789",
                    AppIdentityUserId = identityUser3.Id,
                    FirstName = "Łukasz",
                    LastName = "Biernat",
                    IsActive = true
                };
                Doctor doctor3 = new Doctor()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    DocumentDate = new DateTime(2015, 6, 1),
                    DocumentNumber = "7895740",
                    Person = user3,
                    Specialization = specialization3,
                    Title = "Lek. Med."
                };
                context.Doctors.Add(doctor3);

                AppIdentityUser identityUser4 = new AppIdentityUser() { UserName = "marlenadlugosz@calmedic.pl", Email = "marlenadlugosz@calmedic.pl", EmailConfirmed = true };
                IdentityResult identityResult4 = userManager.CreateAsync(identityUser4, "Test.1234").Result;
                AppUser user4 = new AppUser()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    AvatarUrl = "4.png",
                    Email = "marlenadlugosz@calmedic.pl",
                    PhoneNumber = "+48 123 456 789",
                    AppIdentityUserId = identityUser4.Id,
                    FirstName = "Marlena",
                    LastName = "Długosz-Ptak",
                    IsActive = true
                };
                Doctor doctor4 = new Doctor()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    DocumentDate = new DateTime(2015, 6, 1),
                    DocumentNumber = "7123440",
                    Person = user4,
                    Specialization = specialization4,
                    Title = "Lek. Med."
                };
                context.Doctors.Add(doctor4);

                AppIdentityUser identityUser5 = new AppIdentityUser() { UserName = "mariolapartyka@calmedic.pl", Email = "mariolapartyka@calmedic.pl", EmailConfirmed = true };
                IdentityResult identityResult5 = userManager.CreateAsync(identityUser5, "Test.1234").Result;
                AppUser user5 = new AppUser()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    AvatarUrl = "5.png",
                    Email = "mariolapartyka@calmedic.pl",
                    PhoneNumber = "+48 123 456 789",
                    AppIdentityUserId = identityUser5.Id,
                    FirstName = "Mariola",
                    LastName = "Partya",
                    IsActive = true
                };
                Doctor doctor5 = new Doctor()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    DocumentDate = new DateTime(2015, 6, 1),
                    DocumentNumber = "9923440",
                    Person = user5,
                    Specialization = specialization5,
                    Title = "Lek. Med."
                };
                context.Doctors.Add(doctor5);

                AppIdentityUser identityUser6 = new AppIdentityUser() { UserName = "stanislawadamczyk@calmedic.pl", Email = "stanislawadamczyk@calmedic.pl", EmailConfirmed = true };
                IdentityResult identityResult6 = userManager.CreateAsync(identityUser6, "Test.1234").Result;
                AppUser user6 = new AppUser()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    AvatarUrl = "6.png",
                    Email = "stanislawadamczyk@calmedic.pl",
                    PhoneNumber = "+48 123 456 789",
                    AppIdentityUserId = identityUser6.Id,
                    FirstName = "Stanisław",
                    LastName = "Adamczyk",
                    IsActive = true
                };
                Doctor doctor6 = new Doctor()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    DocumentDate = new DateTime(2015, 6, 1),
                    DocumentNumber = "1923440",
                    Person = user6,
                    Specialization = specialization2,
                    Title = "Prof. Dr Hab."
                };
                context.Doctors.Add(doctor6);

                AppIdentityUser identityUser7 = new AppIdentityUser() { UserName = "jankrol@calmedic.pl", Email = "jankrol@calmedic.pl", EmailConfirmed = true };
                IdentityResult identityResult7 = userManager.CreateAsync(identityUser7, "Test.1234").Result;
                AppUser user7 = new AppUser()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    AvatarUrl = "7.png",
                    Email = "jankrol@calmedic.pl",
                    PhoneNumber = "+48 123 456 789",
                    AppIdentityUserId = identityUser7.Id,
                    FirstName = "Jan",
                    LastName = "Król",
                    IsActive = true
                };
                Doctor doctor7 = new Doctor()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    DocumentDate = new DateTime(2015, 6, 1),
                    DocumentNumber = "1234414",
                    Person = user7,
                    Specialization = specialization3,
                    Title = "Dr Hab. N. Med."
                };
                context.Doctors.Add(doctor7);

                AppIdentityUser identityUser8 = new AppIdentityUser() { UserName = "karolinaszczepanska@calmedic.pl", Email = "karolinaszczepanska@calmedic.pl", EmailConfirmed = true };
                IdentityResult identityResult8 = userManager.CreateAsync(identityUser8, "Test.1234").Result;
                AppUser user8 = new AppUser()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    AvatarUrl = "8.png",
                    Email = "karolinaszczepanska@calmedic.pl",
                    PhoneNumber = "+48 123 456 789",
                    AppIdentityUserId = identityUser8.Id,
                    FirstName = "Karolina",
                    LastName = "Szczepańska",
                    IsActive = true
                };
                Doctor doctor8 = new Doctor()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    DocumentDate = new DateTime(2015, 6, 1),
                    DocumentNumber = "7893414",
                    Person = user8,
                    Specialization = specialization4,
                    Title = "Dr Hab. N. Med."
                };
                context.Doctors.Add(doctor8);
                context.SaveChanges();

                ClinicUser clinicUser1 = new ClinicUser()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Clinic = clinic,
                    User = user1
                };
                context.ClinicUsers.Add(clinicUser1);
                ClinicUser clinicUser2 = new ClinicUser()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Clinic = clinic,
                    User = user2
                };
                context.ClinicUsers.Add(clinicUser2);
                ClinicUser clinicUser3 = new ClinicUser()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Clinic = clinic,
                    User = user3
                };
                context.ClinicUsers.Add(clinicUser3);
                ClinicUser clinicUser4 = new ClinicUser()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Clinic = clinic,
                    User = user4
                };
                context.ClinicUsers.Add(clinicUser4);
                ClinicUser clinicUser5 = new ClinicUser()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Clinic = clinic,
                    User = user5
                };
                context.ClinicUsers.Add(clinicUser5);
                ClinicUser clinicUser6 = new ClinicUser()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Clinic = clinic,
                    User = user6
                };
                context.ClinicUsers.Add(clinicUser6);
                ClinicUser clinicUser7 = new ClinicUser()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Clinic = clinic,
                    User = user7
                };
                context.ClinicUsers.Add(clinicUser7);
                ClinicUser clinicUser8 = new ClinicUser()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Clinic = clinic,
                    User = user8
                };
                context.ClinicUsers.Add(clinicUser8);

                AppUserRole appUserRole1 = new AppUserRole()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    AppRoleId = doctorRole.Id,
                    AppUserId = user1.Id,
                };
                context.AppUserRoles.Add(appUserRole1);

                AppUserRole appUserRole2 = new AppUserRole()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    AppRoleId = doctorRole.Id,
                    AppUserId = user2.Id,
                };
                context.AppUserRoles.Add(appUserRole2);

                AppUserRole appUserRole3 = new AppUserRole()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    AppRoleId = doctorRole.Id,
                    AppUserId = user3.Id,
                };
                context.AppUserRoles.Add(appUserRole3);

                AppUserRole appUserRole4 = new AppUserRole()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    AppRoleId = doctorRole.Id,
                    AppUserId = user4.Id,
                };
                context.AppUserRoles.Add(appUserRole4);

                AppUserRole appUserRole5 = new AppUserRole()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    AppRoleId = doctorRole.Id,
                    AppUserId = user5.Id,
                };
                context.AppUserRoles.Add(appUserRole5);

                AppUserRole appUserRole6 = new AppUserRole()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    AppRoleId = doctorRole.Id,
                    AppUserId = user6.Id,
                };
                context.AppUserRoles.Add(appUserRole6);

                AppUserRole appUserRole7 = new AppUserRole()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    AppRoleId = doctorRole.Id,
                    AppUserId = user7.Id,
                };
                context.AppUserRoles.Add(appUserRole7);

                AppUserRole appUserRole8 = new AppUserRole()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    AppRoleId = doctorRole.Id,
                    AppUserId = user8.Id,
                };
                context.AppUserRoles.Add(appUserRole8);



                Visit visit1 = new Visit()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Clinic = clinic,
                    DateFrom = DateTime.Today.AddHours(8),
                    DateTo = DateTime.Today.AddHours(8).AddMinutes(30),
                    Doctor = doctor1,
                    Patient = patient1,
                    Description = "Od dwóch dni u pacjenta występują omdlenia po wysiłku fizycznym. Nie choruje przewlekle.",
                    Status = VisitStatus.Finished
                };
                context.Visits.Add(visit1);

                Visit visit2 = new Visit()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Clinic = clinic,
                    DateFrom = DateTime.Today.AddHours(8),
                    DateTo = DateTime.Today.AddHours(8).AddMinutes(30),
                    Doctor = doctor2,
                    Patient = patient2,
                    Description = "",
                    Status = VisitStatus.Canceled
                };
                context.Visits.Add(visit2);

                Visit visit3 = new Visit()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Clinic = clinic,
                    DateFrom = DateTime.Today.AddHours(8),
                    DateTo = DateTime.Today.AddHours(8).AddMinutes(30),
                    Doctor = doctor3,
                    Patient = patient3,
                    Description = "",
                    Status = VisitStatus.Finished
                };
                context.Visits.Add(visit3);

                Visit visit4 = new Visit()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Clinic = clinic,
                    DateFrom = DateTime.Today.AddHours(8).AddMinutes(0),
                    DateTo = DateTime.Today.AddHours(8).AddMinutes(30),
                    Doctor = doctor4,
                    Patient = patient4,
                    Description = "U pacjenta od 3 dni występują ostre biegunki oraz wymiotowanie krwią. Pacjent cierpi na zapalenie jelita grubego.",
                    Status = VisitStatus.Finished
                };
                context.Visits.Add(visit4);

                Visit visit5 = new Visit()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Clinic = clinic,
                    DateFrom = DateTime.Today.AddHours(8).AddMinutes(15),
                    DateTo = DateTime.Today.AddHours(8).AddMinutes(30),
                    Doctor = doctor5,
                    Patient = patient5,
                    Description = "Pacjent od kilku dni obserwuje zawroty głowy oraz ból w klatce piersiowej. Choruje na zaburzenia rytmu serca.",
                    Status = VisitStatus.Finished
                };
                context.Visits.Add(visit4);

                Visit visit6 = new Visit()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Clinic = clinic,
                    DateFrom = DateTime.Today.AddHours(8).AddMinutes(30),
                    DateTo = DateTime.Today.AddHours(8).AddMinutes(45),
                    Doctor = doctor6,
                    Patient = patient6,
                    Description = "",
                    Status = VisitStatus.Canceled
                };
                context.Visits.Add(visit6);

                Visit visit7 = new Visit()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Clinic = clinic,
                    DateFrom = DateTime.Today.AddHours(11).AddMinutes(30),
                    DateTo = DateTime.Today.AddHours(12).AddMinutes(5),
                    Doctor = doctor7,
                    Patient = patient7,
                    Description = "",
                    Status = VisitStatus.Canceled
                };
                context.Visits.Add(visit7);

                Visit visit8 = new Visit()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Clinic = clinic,
                    DateFrom = DateTime.Today.AddHours(10).AddMinutes(30),
                    DateTo = DateTime.Today.AddHours(10).AddMinutes(45),
                    Doctor = doctor8,
                    Patient = patient8,
                    Description = "",
                    Status = VisitStatus.Finished
                };
                context.Visits.Add(visit8);

                Visit visit9 = new Visit()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Clinic = clinic,
                    DateFrom = DateTime.Today.AddHours(9).AddMinutes(30),
                    DateTo = DateTime.Today.AddHours(9).AddMinutes(45),
                    Doctor = doctor1,
                    Patient = patient3,
                    Description = "",
                    Status = VisitStatus.Waiting
                };
                context.Visits.Add(visit9);

                Visit visit10 = new Visit()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Clinic = clinic,
                    DateFrom = DateTime.Today.AddHours(9).AddMinutes(50),
                    DateTo = DateTime.Today.AddHours(10).AddMinutes(20),
                    Doctor = doctor1,
                    Patient = patient9,
                    Description = "Pacjent od dłuższego czasu obserwuje problemy z samopoczuciem oraz snem. Jednocześnie pacjent nie cierpi na żadne choroby przewlekłe.",
                    Status = VisitStatus.Waiting
                };
                context.Visits.Add(visit9);

                Visit visit11 = new Visit()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Clinic = clinic,
                    DateFrom = DateTime.Today.AddHours(9).AddMinutes(50),
                    DateTo = DateTime.Today.AddHours(10).AddMinutes(20),
                    Doctor = doctor2,
                    Patient = patient10,
                    Description = "Od dwóch dni u pacjenta występują omdlenia po wysiłku fizycznym. Nie choruje przewlekle.",
                    Status = VisitStatus.Waiting
                };
                context.Visits.Add(visit11);

                Visit visit12 = new Visit()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Clinic = clinic,
                    DateFrom = DateTime.Today.AddHours(12).AddMinutes(20),
                    DateTo = DateTime.Today.AddHours(12).AddMinutes(40),
                    Doctor = doctor2,
                    Patient = patient10,
                    Description = "Pacjent od kilku dni obserwuje zawroty głowy oraz ból w klatce piersiowej. Choruje na zaburzenia rytmu serca.",
                    Status = VisitStatus.Waiting
                };
                context.Visits.Add(visit12);

                Visit visit13 = new Visit()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Clinic = clinic,
                    DateFrom = DateTime.Today.AddHours(12).AddMinutes(10),
                    DateTo = DateTime.Today.AddHours(12).AddMinutes(40),
                    Doctor = doctor3,
                    Patient = patient11,
                    Description = "",
                    Status = VisitStatus.Canceled
                };
                context.Visits.Add(visit13);

                Visit visit14 = new Visit()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Clinic = clinic,
                    DateFrom = DateTime.Today.AddHours(12).AddMinutes(50),
                    DateTo = DateTime.Today.AddHours(13).AddMinutes(10),
                    Doctor = doctor3,
                    Patient = patient12,
                    Description = "",
                    Status = VisitStatus.Waiting
                };
                context.Visits.Add(visit14);

                Visit visit15 = new Visit()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Clinic = clinic,
                    DateFrom = DateTime.Today.AddHours(10).AddMinutes(50),
                    DateTo = DateTime.Today.AddHours(11).AddMinutes(10),
                    Doctor = doctor4,
                    Patient = patient13,
                    Description = "Pacjent od dłuższego czasu obserwuje problemy z samopoczuciem oraz snem. Jednocześnie pacjent nie cierpi na żadne choroby przewlekłe.",
                    Status = VisitStatus.Waiting
                };
                context.Visits.Add(visit15);

                Visit visit16 = new Visit()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Clinic = clinic,
                    DateFrom = DateTime.Today.AddHours(11).AddMinutes(15),
                    DateTo = DateTime.Today.AddHours(11).AddMinutes(45),
                    Doctor = doctor4,
                    Patient = patient13,
                    Description = "U pacjenta od 3 dni występują ostre biegunki oraz wymiotowanie krwią. Pacjent cierpi na zapalenie jelita grubego.",
                    Status = VisitStatus.Waiting
                };
                context.Visits.Add(visit16);

                Visit visit17 = new Visit()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Clinic = clinic,
                    DateFrom = DateTime.Today.AddHours(11).AddMinutes(15),
                    DateTo = DateTime.Today.AddHours(11).AddMinutes(45),
                    Doctor = doctor5,
                    Patient = patient14,
                    Description = "U pacjenta od 3 dni występują ostre biegunki oraz wymiotowanie krwią. Pacjent cierpi na zapalenie jelita grubego.",
                    Status = VisitStatus.Waiting
                };
                context.Visits.Add(visit17);

                Visit visit18 = new Visit()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Clinic = clinic,
                    DateFrom = DateTime.Today.AddHours(11).AddMinutes(45),
                    DateTo = DateTime.Today.AddHours(12).AddMinutes(0),
                    Doctor = doctor5,
                    Patient = patient15,
                    Description = "",
                    Status = VisitStatus.Waiting
                };
                context.Visits.Add(visit18);

                Visit visit19 = new Visit()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Clinic = clinic,
                    DateFrom = DateTime.Today.AddHours(10).AddMinutes(45),
                    DateTo = DateTime.Today.AddHours(11).AddMinutes(0),
                    Doctor = doctor6,
                    Patient = patient16,
                    Description = "U pacjenta od kilku dni występuje ostry, kłujący ból brzucha po prawej stronie. Pacjent nie choruje przewlekle.",
                    Status = VisitStatus.Waiting
                };
                context.Visits.Add(visit19);

                Visit visit20 = new Visit()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Clinic = clinic,
                    DateFrom = DateTime.Today.AddHours(11).AddMinutes(15).AddDays(-1),
                    DateTo = DateTime.Today.AddHours(11).AddMinutes(45).AddDays(-1),
                    Doctor = doctor5,
                    Patient = patient1,
                    Description = "U pacjenta od 3 dni występują ostre biegunki oraz wymiotowanie krwią. Pacjent cierpi na zapalenie jelita grubego.",
                    Status = VisitStatus.Finished
                };
                context.Visits.Add(visit20);

                Visit visit21 = new Visit()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Clinic = clinic,
                    DateFrom = DateTime.Today.AddHours(11).AddMinutes(45).AddDays(-1),
                    DateTo = DateTime.Today.AddHours(12).AddMinutes(0).AddDays(-1),
                    Doctor = doctor5,
                    Patient = patient12,
                    Description = "",
                    Status = VisitStatus.Finished
                };
                context.Visits.Add(visit21);

                Visit visit22 = new Visit()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Clinic = clinic,
                    DateFrom = DateTime.Today.AddHours(11).AddMinutes(45).AddDays(-1),
                    DateTo = DateTime.Today.AddHours(12).AddMinutes(0).AddDays(-1),
                    Doctor = doctor2,
                    Patient = patient12,
                    Description = "",
                    Status = VisitStatus.Finished
                };
                context.Visits.Add(visit21);

                Visit visit23 = new Visit()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Clinic = clinic,
                    DateFrom = DateTime.Today.AddHours(11).AddMinutes(45).AddDays(-1),
                    DateTo = DateTime.Today.AddHours(12).AddMinutes(0).AddDays(-1),
                    Doctor = doctor1,
                    Patient = patient12,
                    Description = "",
                    Status = VisitStatus.Finished
                };
                context.Visits.Add(visit23);

                Visit visit24 = new Visit()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Clinic = clinic,
                    DateFrom = DateTime.Today.AddHours(13).AddMinutes(45).AddDays(-1),
                    DateTo = DateTime.Today.AddHours(14).AddMinutes(0).AddDays(-1),
                    Doctor = doctor1,
                    Patient = patient14,
                    Description = "",
                    Status = VisitStatus.Finished
                };
                context.Visits.Add(visit24);

                Visit visit25 = new Visit()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Clinic = clinic,
                    DateFrom = DateTime.Today.AddHours(13).AddMinutes(45).AddDays(-1),
                    DateTo = DateTime.Today.AddHours(14).AddMinutes(0).AddDays(-1),
                    Doctor = doctor5,
                    Patient = patient17,
                    Description = "",
                    Status = VisitStatus.Finished
                };
                context.Visits.Add(visit25);

                Visit visit26 = new Visit()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Clinic = clinic,
                    DateFrom = DateTime.Today.AddHours(13).AddMinutes(45).AddDays(1),
                    DateTo = DateTime.Today.AddHours(14).AddMinutes(0).AddDays(1),
                    Doctor = doctor5,
                    Patient = patient16,
                    Description = "",
                    Status = VisitStatus.Waiting
                };
                context.Visits.Add(visit26);

                Visit visit27 = new Visit()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Clinic = clinic,
                    DateFrom = DateTime.Today.AddHours(11).AddMinutes(45).AddDays(1),
                    DateTo = DateTime.Today.AddHours(12).AddMinutes(0).AddDays(1),
                    Doctor = doctor5,
                    Patient = patient15,
                    Description = "",
                    Status = VisitStatus.Waiting
                };
                context.Visits.Add(visit27);

                Visit visit28 = new Visit()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Clinic = clinic,
                    DateFrom = DateTime.Today.AddHours(11).AddMinutes(45).AddDays(1),
                    DateTo = DateTime.Today.AddHours(12).AddMinutes(0).AddDays(1),
                    Doctor = doctor7,
                    Patient = patient2,
                    Description = "",
                    Status = VisitStatus.Waiting
                };
                context.Visits.Add(visit28);

                Visit visit29 = new Visit()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Clinic = clinic,
                    DateFrom = DateTime.Today.AddHours(10).AddMinutes(45).AddDays(1),
                    DateTo = DateTime.Today.AddHours(11).AddMinutes(15).AddDays(1),
                    Doctor = doctor7,
                    Patient = patient1,
                    Description = "",
                    Status = VisitStatus.Waiting
                };
                context.Visits.Add(visit29);

                Visit visit30 = new Visit()
                {
                    CreatedById = sysAdmin.CreatedById,
                    CreatedDate = DateTime.Now,
                    Clinic = clinic,
                    DateFrom = DateTime.Today.AddHours(10).AddMinutes(45).AddDays(1),
                    DateTo = DateTime.Today.AddHours(11).AddMinutes(15).AddDays(1),
                    Doctor = doctor1,
                    Patient = patient7,
                    Description = "",
                    Status = VisitStatus.Waiting
                };
                context.Visits.Add(visit30);
            }
            context.SaveChanges();
        }

        #endregion Test
    }
}