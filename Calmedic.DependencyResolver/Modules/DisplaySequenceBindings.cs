using Autofac;
using Autofac.Extras.DynamicProxy;
using Calmedic.Application;
using Calmedic.Data;

namespace Calmedic.DependencyResolver
{
    internal class DisplaySequenceBindings
    {
        internal static void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DisplaySequenceService>().As<IDisplaySequenceService>().EnableClassInterceptors().InterceptedBy(typeof(TransactionInterceptor)).PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).InstancePerLifetimeScope();
            builder.RegisterType<DisplaySequenceService>().As<DisplaySequenceService>().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).InstancePerLifetimeScope();

            builder.RegisterType<DisplaySequenceConverter>().AsSelf().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).InstancePerLifetimeScope();

            builder.RegisterType<DisplaySequenceRepository>().As<IDisplaySequenceRepository>().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).InstancePerLifetimeScope();
            builder.RegisterType<DisplaySequenceRepository>().As<DisplaySequenceRepository>().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).InstancePerLifetimeScope();
        }
    }
}