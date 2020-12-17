using Autofac;
using Autofac.Extras.DynamicProxy;
using Calmedic.Application;
using Calmedic.Data;

namespace Calmedic.DependencyResolver
{
    internal class ClinicBindings
    {
        internal static void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ClinicService>().As<IClinicService>().EnableClassInterceptors().InterceptedBy(typeof(TransactionInterceptor)).PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).InstancePerLifetimeScope();
            builder.RegisterType<ClinicService>().As<ClinicService>().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).InstancePerLifetimeScope();

            builder.RegisterType<ClinicConverter>().AsSelf().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).InstancePerLifetimeScope();

            builder.RegisterType<ClinicRepository>().As<IClinicRepository>().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).InstancePerLifetimeScope();
            builder.RegisterType<ClinicRepository>().As<ClinicRepository>().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).InstancePerLifetimeScope();

            builder.RegisterType<ClinicUserRepository>().As<IClinicUserRepository>().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).InstancePerLifetimeScope();
            builder.RegisterType<ClinicUserRepository>().As<ClinicUserRepository>().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).InstancePerLifetimeScope();
        }
    }
}