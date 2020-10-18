using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Calmedic.EntityFramework
{
    public class AppContextFactory : IDesignTimeDbContextFactory<MainDatabaseContext>
    {
        private static DbContextOptions<MainDatabaseContext> _options = null;

        public AppContextFactory()
        {
            // A parameter-less constructor is required by the EF Core CLI tools.
            if (_options == null)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .Build();

                var builder = new DbContextOptionsBuilder<MainDatabaseContext>();
                var connectionString = configuration.GetConnectionString("MainDatabaseContext");

                builder.UseSqlServer(connectionString);
                builder.UseLazyLoadingProxies();
                _options = builder.Options;
            }
        }

        public MainDatabaseContext CreateDbContext(string[] args)
        {
            return new MainDatabaseContext(_options);
        }
    }
}
