using Autofac;
using Autofac.Extras.DynamicProxy;
using Calmedic.Application;

namespace Calmedic.DependencyResolver
{
    internal class DashboardBindings
    {
        internal static void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DashboardService>().As<IDashboardService>().EnableClassInterceptors().InterceptedBy(typeof(TransactionInterceptor)).PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).InstancePerLifetimeScope();
            builder.RegisterType<DashboardService>().As<DashboardService>().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).InstancePerLifetimeScope();

            builder.RegisterType<DashboardConverter>().AsSelf().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).InstancePerLifetimeScope();
        }
    }
}