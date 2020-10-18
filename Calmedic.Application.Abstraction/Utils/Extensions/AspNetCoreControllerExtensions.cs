using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Calmedic.Utils
{
    public static class AspNetCoreControllerExtensions
    {
        public static void AddCustomControllerActivation(this IServiceCollection services, Func<Type, object> activator)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (activator == null) throw new ArgumentNullException(nameof(activator));

            services.AddSingleton<IControllerActivator>(new DelegatingControllerActivator(
                context => activator(context.ActionDescriptor.ControllerTypeInfo.AsType())
                )
            );
        }

        public static Type[] GetControllerTypes(this IApplicationBuilder builder)
        {
            var manager = builder.ApplicationServices.GetRequiredService<ApplicationPartManager>();

            var feature = new ControllerFeature();
            manager.PopulateFeature(feature);

            return feature.Controllers.Select(t => t.AsType()).ToArray();
        }
    }

    public sealed class DelegatingControllerActivator : IControllerActivator
    {
        public DelegatingControllerActivator(Func<ControllerContext, object> controllerCreator, Action<ControllerContext, object> controllerReleaser = null)
        {
            this.controllerCreator = controllerCreator ?? throw new ArgumentNullException(nameof(controllerCreator));
            this.controllerReleaser = controllerReleaser ?? ((_, __) => { });
        }

        private readonly Func<ControllerContext, object> controllerCreator;
        private readonly Action<ControllerContext, object> controllerReleaser;

        public object Create(ControllerContext context) => this.controllerCreator(context);
        public void Release(ControllerContext context, object controller) => this.controllerReleaser(context, controller);
    }
}
