using Autofac;
using Hangfire;
using System;

namespace Calmedic.Jobs
{
    internal class InternalJobActivatorScope : JobActivatorScope
    {
        private ILifetimeScope _scope;

        public InternalJobActivatorScope(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public override object Resolve(Type type)
        {
            return _scope.Resolve(type);
        }

        public override void DisposeScope()
        {
            _scope.Dispose();
        }
    }
}