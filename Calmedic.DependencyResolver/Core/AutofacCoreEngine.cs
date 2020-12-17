using Autofac;
using Calmedic.Application;
using Calmedic.EntityFramework;
using Calmedic.Utils;

namespace Calmedic.DependencyResolver
{
    public class AutofacCoreEngine : Module
    {
        public AutofacCoreEngine()
        { }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MainContext>().AsSelf().PropertiesAutowired().InstancePerLifetimeScope();

            builder.RegisterType<TransactionInterceptor>().AsSelf().PropertiesAutowired().InstancePerLifetimeScope();
            builder.RegisterType<DbSession>().AsSelf().PropertiesAutowired().InstancePerLifetimeScope();

            builder.Register(context => MainDatabaseContext.Create()).As<MainDatabaseContext>().PropertiesAutowired().InstancePerLifetimeScope();

            builder.RegisterType<TransactionInterceptor>().InstancePerLifetimeScope();

            ClinicBindings.Load(builder);
            CoreBindings.Load(builder); 
            DisplaySequenceBindings.Load(builder);
            DoctorBindings.Load(builder);
            PatientBindings.Load(builder);
            MembershipBindings.Load(builder);
            VisitBindings.Load(builder);
        }
    }
}
