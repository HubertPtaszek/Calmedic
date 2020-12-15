using Autofac;
using Hangfire;

namespace Calmedic.Jobs
{
    public class HangfireActivator : JobActivator
    {
        private readonly ILifetimeScope _lifetimeScope;

        public HangfireActivator(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public override JobActivatorScope BeginScope(JobActivatorContext context)
        {
            ILifetimeScope scope = _lifetimeScope.BeginLifetimeScope();
            return new InternalJobActivatorScope(scope);
        }
    }
}