using Autofac;
using Autofac.Extras.DynamicProxy;
using Calmedic.Application;
using Calmedic.Data;

namespace Calmedic.DependencyResolver
{
    internal class CoreBindings
    {
        internal static void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AppSettingsService>().As<IAppSettingsService>().InstancePerLifetimeScope().EnableClassInterceptors().InterceptedBy(typeof(TransactionInterceptor)).PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            builder.RegisterType<AppSettingsService>().As<AppSettingsService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.RegisterType<AppSettingsRepository>().As<IAppSettingsRepository>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            builder.RegisterType<AppSettingsRepository>().As<AppSettingsRepository>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.RegisterType<AppMailMessageService>().As<IAppMailMessageService>().InstancePerLifetimeScope().EnableClassInterceptors().InterceptedBy(typeof(TransactionInterceptor)).PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            builder.RegisterType<AppMailMessageService>().As<AppMailMessageService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.RegisterType<AppMailMessageConverter>().AsSelf().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).InstancePerLifetimeScope();

            builder.RegisterType<AppMailMessageRepository>().As<IAppMailMessageRepository>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            builder.RegisterType<AppMailMessageRepository>().As<AppMailMessageRepository>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.RegisterType<FileService>().As<IFileService>().InstancePerLifetimeScope().EnableClassInterceptors().InterceptedBy(typeof(TransactionInterceptor)).PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            builder.RegisterType<FileService>().As<FileService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.RegisterType<FileConverter>().AsSelf().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).InstancePerLifetimeScope();

            builder.RegisterType<FileRepository>().As<IFileRepository>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            builder.RegisterType<FileRepository>().As<FileRepository>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.RegisterType<AddressConverter>().AsSelf().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).InstancePerLifetimeScope();
        }
    }
}