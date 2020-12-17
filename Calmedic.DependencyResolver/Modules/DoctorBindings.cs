using Autofac;
using Autofac.Extras.DynamicProxy;
using Calmedic.Application;
using Calmedic.Data;

namespace Calmedic.DependencyResolver
{
    internal class DoctorBindings
    {
        internal static void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DoctorService>().As<IDoctorService>().EnableClassInterceptors().InterceptedBy(typeof(TransactionInterceptor)).PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).InstancePerLifetimeScope();
            builder.RegisterType<DoctorService>().As<DoctorService>().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).InstancePerLifetimeScope();

            builder.RegisterType<DoctorConverter>().AsSelf().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).InstancePerLifetimeScope();

            builder.RegisterType<DoctorRepository>().As<IDoctorRepository>().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).InstancePerLifetimeScope();
            builder.RegisterType<DoctorRepository>().As<DoctorRepository>().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).InstancePerLifetimeScope();

            builder.RegisterType<SpecializationRepository>().As<ISpecializationRepository>().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).InstancePerLifetimeScope();
            builder.RegisterType<SpecializationRepository>().As<SpecializationRepository>().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).InstancePerLifetimeScope();
        }
    }
}