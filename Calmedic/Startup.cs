using Autofac;
using Autofac.Extensions.DependencyInjection;
using Calmedic.DependencyResolver;
using Calmedic.Domain;
using Calmedic.EntityFramework;
using Calmedic.Jobs;
using Calmedic.Jobs.JobSection;
using Calmedic.Utils;
using Hangfire;
using Hangfire.States;
using Hangfire.Storage;
using Hangfire.Storage.Monitoring;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Calmedic
{
    public class Startup
    {
        public ILifetimeScope AutofacContainer { get; private set; }

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
              .SetBasePath(env.ContentRootPath)
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
              .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
              .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }
        private List<TypeInfo> _controllerTypes = new List<TypeInfo>();
        private BackgroundJobServer _backgroundJobServer;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.AddDbContext<MainDatabaseContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("MainDatabaseContext")));
            services.AddDefaultIdentity<AppIdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<MainDatabaseContext>();

            services.AddLocalization();

            var mvcBuilder = services.AddControllersWithViews().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null)
                            .AddNewtonsoftJson()
                            .AddSessionStateTempDataProvider()
                            .AddRazorRuntimeCompilation()
                            .AddControllersAsServices();

            services.AddRazorPages();
            services.AddSession();

            var controllerFeature = new ControllerFeature();
            mvcBuilder.PartManager.PopulateFeature(controllerFeature);
            _controllerTypes = controllerFeature.Controllers.ToList();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Register your own things directly with Autofac, like:
            builder.RegisterModule<AutofacCoreEngine>();
            foreach (var controllerType in _controllerTypes)
            {
                builder.RegisterType(controllerType).PropertiesAutowired();
            }
            builder.RegisterType<SendEmailJob>().PropertiesAutowired();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider, IHostApplicationLifetime appLifetime)
        {
            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            var supportedCultures = new[]
            {
                new CultureInfo("pl-PL"),
                new CultureInfo("en-GB"),
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("pl-PL"),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute("AdminPanel_default", AreaNames.DisplaySequence_Area,
                  AreaNames.DisplaySequence_Area + "/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute("Membership_default", AreaNames.Membership_Area,
                    AreaNames.Membership_Area + "/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute("Evidence_default", AreaNames.Evidence_Area,
                   AreaNames.Evidence_Area + "/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute("Gallery_default", AreaNames.Gallery_Area,
                   AreaNames.Gallery_Area + "/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute("Presentation_default", AreaNames.Presentation_Area,
                   "/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                var mainContext = scope.ServiceProvider.GetRequiredService<MainDatabaseContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppIdentityUser>>();
                DbInitializer.Seed(mainContext, userManager);
            }

            GlobalConfiguration.Configuration
             .UseSqlServerStorage(Configuration.GetConnectionString("MainDatabaseContext"))
             .UseActivator(new HangfireActivator(this.AutofacContainer));
            _backgroundJobServer = new BackgroundJobServer();
            if (JobStorage.Current?.GetMonitoringApi() != null)
                PurgeJobs(JobStorage.Current?.GetMonitoringApi());
            HangfireJobSection jobSection = Configuration.GetSection("HangfireJobSection").Get<HangfireJobSection>();
            if (jobSection != null)
            {
                ActiveJobs(jobSection);
            }
            appLifetime.ApplicationStopping.Register(() =>
            {
                if (_backgroundJobServer != null)
                {
                    _backgroundJobServer.Dispose();
                }
            });
        }

        private void ActiveJobs(HangfireJobSection jobSection)
        {
            ManageJob<SendEmailJob>("1", jobSection);
        }

        private void ManageJob<T>(string jobId, HangfireJobSection jobSection) where T : Job
        {
            HangfireJobSectionItem jobInfo = jobSection.Jobs.FirstOrDefault(x => x.JobId == jobId);
            if (jobInfo == null || !jobInfo.IsEnabled || !jobSection.IsEnabled)
            {
                RecurringJob.RemoveIfExists(jobId);
            }
            else
            {
                TimeZoneInfo timeZone = TimeZoneInfo.Local;
                string queue = jobInfo.Queue.IsNullOrEmpty() ? EnqueuedState.DefaultQueue : jobInfo.Queue;
                RecurringJob.AddOrUpdate<T>(jobId, x => x.Execute(), jobInfo.CronExpression, timeZone: timeZone, queue: queue);
            }
        }

        private void PurgeJobs(IMonitoringApi monitor)
        {
            var toDelete = new List<string>();

            foreach (QueueWithTopEnqueuedJobsDto queue in monitor.Queues())
            {
                for (var i = 0; i < Math.Ceiling(queue.Length / 1000d); i++)
                {
                    monitor.EnqueuedJobs(queue.Name, 1000 * i, 1000)
                        .ForEach(x => toDelete.Add(x.Key));
                }
            }

            foreach (var jobId in toDelete)
            {
                BackgroundJob.Delete(jobId);
            }
        }
    }
}