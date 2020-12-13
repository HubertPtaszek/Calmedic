using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Calmedic.Domain;
using System;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Calmedic.EntityFramework
{
    public class MainDatabaseContext : IdentityDbContext<AppIdentityUser>
    {
        public IConfiguration Configuration { get; set; }

        public MainDatabaseContext() : base()
        {
            Id = Guid.NewGuid(); 
        }

        public MainDatabaseContext(DbContextOptions<MainDatabaseContext> options) : base(options)
        {
            Id = Guid.NewGuid();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLazyLoadingProxies();
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("MainDatabaseContext"));
            }
        }

        public Guid Id { get; set; }

        public static MainDatabaseContext Create()
        {
            return (new AppContextFactory()).CreateDbContext(new string[0]);
        }

        //Add-Migration -Context MainDatabaseContext -o MainDatabaseMigrations <Nazwa migracji>
        //Update-Database -Context MainDatabaseContext
        //Remove-Migration -Context MainDatabaseContext

        #region Core
        public DbSet<AppSetting> AppSettings { get; set; }
        public DbSet<AppMailMessage> AppMailMessages { get; set; }
        public DbSet<Person> Peoples { get; set; }
        public DbSet<SystemUser> SystemUsers { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<AppUserRole> AppUserRoles { get; set; }
        #endregion Core

        #region Clinics
        public DbSet<Clinic> Clinics { get; set; }
        #endregion

        #region Visits
        public DbSet<Visit> Visits { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Membership
            modelBuilder.ApplyConfiguration(new PersonConfiguration());
            modelBuilder.ApplyConfiguration(new AppUserConfiguration());

            modelBuilder.ApplyConfiguration(new AppUserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new AppRoleConfiguration());
            #endregion Membership

            #region Clinics
            modelBuilder.ApplyConfiguration(new ClinicConfiguration());
            #endregion

            #region Visits
            modelBuilder.ApplyConfiguration(new VisitConfiguration());
            #endregion

            foreach (var property in modelBuilder.Model.GetEntityTypes()
           .SelectMany(t => t.GetProperties())
           .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18, 5)");
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}
