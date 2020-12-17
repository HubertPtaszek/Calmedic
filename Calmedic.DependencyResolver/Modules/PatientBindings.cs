using Autofac;
using Autofac.Extras.DynamicProxy;
using Calmedic.Application;
using Calmedic.Data;

namespace Calmedic.DependencyResolver
{
    internal class PatientBindings
    {
        internal static void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PatientService>().As<IPatientService>().EnableClassInterceptors().InterceptedBy(typeof(TransactionInterceptor)).PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).InstancePerLifetimeScope();
            builder.RegisterType<PatientService>().As<PatientService>().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).InstancePerLifetimeScope();

            builder.RegisterType<PatientConverter>().AsSelf().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).InstancePerLifetimeScope();

            builder.RegisterType<PatientRepository>().As<IPatientRepository>().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).InstancePerLifetimeScope();
            builder.RegisterType<PatientRepository>().As<PatientRepository>().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).InstancePerLifetimeScope();
        }
    }
}
