using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Autofac;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Calmedic.EntityFramework;
using Calmedic.Domain;
using Calmedic.DependencyResolver;
using System.Globalization;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Localization;
using Calmedic.Utils;

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
                endpoints.MapAreaControllerRoute("AdminPanel_default", AreaNames.AdminPanel_Area,
                  AreaNames.AdminPanel_Area + "/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute("Dashboard_default", AreaNames.Dashboard_Area,
                    AreaNames.Dashboard_Area + "/{controller=Home}/{action=Index}/{id?}");

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
        }
    }
}
