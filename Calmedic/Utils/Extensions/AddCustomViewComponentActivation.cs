using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Calmedic.Utils
{
    public static class AspNetCoreViewsExtensions
    {
        public static void AddCustomViewComponentActivation(this IServiceCollection services, Func<Type, object> activator)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (activator == null) throw new ArgumentNullException(nameof(activator));

            services.AddSingleton<IViewComponentActivator>(new DelegatingViewComponentActivator(activator));
        }

        public static void AddCustomTagHelperActivation(this IServiceCollection services, Func<Type, object> activator,
            Predicate<Type> applicationTypeSelector = null)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (activator == null) throw new ArgumentNullException(nameof(activator));            

            applicationTypeSelector = applicationTypeSelector ?? (type => !type.GetTypeInfo().Namespace.StartsWith("Microsoft"));
        }



        public static Type[] GetViewComponentTypes(this IApplicationBuilder builder)
        {
            var manager = builder.ApplicationServices.GetRequiredService<ApplicationPartManager>();

            var feature = new ViewComponentFeature();
            manager.PopulateFeature(feature);

            return feature.ViewComponents.Select(t => t.AsType()).ToArray();
        }
    }
    public sealed class DelegatingViewComponentActivator : IViewComponentActivator
    {
        private readonly Func<Type, object> viewComponentCreator;
        private readonly Action<object> viewComponentReleaser;

        public DelegatingViewComponentActivator(Func<Type, object> viewComponentCreator,
            Action<object> viewComponentReleaser = null)
        {
            this.viewComponentCreator = viewComponentCreator ?? throw new ArgumentNullException(nameof(viewComponentCreator));
            this.viewComponentReleaser = viewComponentReleaser ?? (_ => { });
        }

        public object Create(ViewComponentContext context) =>
            this.viewComponentCreator(context.ViewComponentDescriptor.TypeInfo.AsType());

        public void Release(ViewComponentContext context, object viewComponent) =>
           this.viewComponentReleaser(viewComponent);
    }

    internal sealed class DelegatingTagHelperActivator : ITagHelperActivator
    {
        private readonly Predicate<Type> customCreatorSelector;
        private readonly Func<Type, object> customTagHelperCreator;
        private readonly ITagHelperActivator defaultTagHelperActivator;

        public DelegatingTagHelperActivator(Predicate<Type> customCreatorSelector, Func<Type, object> customTagHelperCreator,
            ITagHelperActivator defaultTagHelperActivator)
        {
            this.customCreatorSelector = customCreatorSelector ?? throw new ArgumentNullException(nameof(customCreatorSelector));
            this.customTagHelperCreator = customTagHelperCreator ?? throw new ArgumentNullException(nameof(customTagHelperCreator));
            this.defaultTagHelperActivator = defaultTagHelperActivator ?? throw new ArgumentNullException(nameof(defaultTagHelperActivator));
        }

        public TTagHelper Create<TTagHelper>(ViewContext context) where TTagHelper : ITagHelper =>
            this.customCreatorSelector(typeof(TTagHelper))
                ? (TTagHelper)this.customTagHelperCreator(typeof(TTagHelper))
                : defaultTagHelperActivator.Create<TTagHelper>(context);
    }
    //internal class DefaultTagHelperActivator : ITagHelperActivator
    //{
    //    private readonly ITypeActivatorCache _typeActivatorCache;

    //    /// <summary>
    //    /// Instantiates a new <see cref="DefaultTagHelperActivator"/> instance.
    //    /// </summary>
    //    /// <param name="typeActivatorCache">The <see cref="ITypeActivatorCache"/>.</param>
    //    public DefaultTagHelperActivator(ITypeActivatorCache typeActivatorCache)
    //    {
    //        if (typeActivatorCache == null)
    //        {
    //            throw new ArgumentNullException(nameof(typeActivatorCache));
    //        }

    //        _typeActivatorCache = typeActivatorCache;
    //    }

    //    /// <inheritdoc />
    //    public TTagHelper Create<TTagHelper>(ViewContext context)
    //        where TTagHelper : ITagHelper
    //    {
    //        if (context == null)
    //        {
    //            throw new ArgumentNullException(nameof(context));
    //        }

    //        return _typeActivatorCache.CreateInstance<TTagHelper>(
    //            context.HttpContext.RequestServices,
    //            typeof(TTagHelper));
    //    }
    //}
}
