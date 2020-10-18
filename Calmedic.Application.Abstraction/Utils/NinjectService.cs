using Microsoft.Extensions.DependencyInjection;
using System;

namespace Calmedic.Utils
{
    internal sealed class NinjectService
    {
        private Func<Type, object> _activator;
        public NinjectService(Func<Type, object> activator)
        {
            _activator = activator;
        }
        internal T GetServiceFromNinject<T>() where T : class
        {
            return _activator(typeof(T)) as T;
        }
    }

    public static class NinjectServiceExtensions
    {
        public static void AddNinjectServiceManualActivation(this IServiceCollection services, Func<Type, object> activator)
        {
            if (activator == null) throw new ArgumentNullException(nameof(activator));
            services.AddSingleton<NinjectService>(new NinjectService(activator));
        }
    }
}
