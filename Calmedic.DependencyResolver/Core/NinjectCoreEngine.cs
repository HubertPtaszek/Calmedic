using Calmedic.Application;
using Calmedic.EntityFramework;
using Calmedic.Utils;
using Autofac;

namespace Calmedic.DependencyResolver
{
    public class NinjectCoreEngine : Module
    {
        public NinjectCoreEngine()
        { }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MainContext>().AsSelf().PropertiesAutowired().InstancePerLifetimeScope();

            builder.RegisterType<TransactionInterceptor>().AsSelf().PropertiesAutowired().InstancePerLifetimeScope();
            builder.RegisterType<DbSession>().AsSelf().PropertiesAutowired().InstancePerLifetimeScope();

            builder.Register(context => MainDatabaseContext.Create()).As<MainDatabaseContext>().PropertiesAutowired().InstancePerLifetimeScope();

            builder.RegisterType<TransactionInterceptor>().InstancePerLifetimeScope();

            CoreBindings.Load(builder);
            MembershipBindings.Load(builder);
            EvidenceBindings.Load(builder);
        }
    }
}
