using Calmedic.Domain;
using Calmedic.EntityFramework;
using Calmedic.Utils;
using Hangfire;
using NLog;
using System;
using System.Diagnostics;
using System.Linq;

namespace Calmedic.Jobs
{
    public abstract class Job
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        protected readonly IServiceProvider _serviceProvider;

        public Job(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected void LogError(string msg, Exception ex)
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);
            logger.Error(ex, string.Format(Loggers.LogFormat, DateTime.Now.ToDateTimeStringSafe(), this.GetType().Name, sf.GetMethod().Name, msg));
        }

        protected void LogInfo(string msg)
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);
            logger.Info(string.Format(Loggers.LogFormat, DateTime.Now.ToDateTimeStringSafe(), this.GetType().Name, sf.GetMethod().Name, msg));
        }

        protected abstract void ExecuteMe();

        [DisableConcurrentExecution(60 * 10)]
        public void Execute()
        {
            MainDatabaseContext mainDbContext = _serviceProvider.GetService(typeof(MainDatabaseContext)) as MainDatabaseContext;
            MainContext mainContext = _serviceProvider.GetService(typeof(MainContext)) as MainContext;
            SystemUser user = mainDbContext.SystemUsers.FirstOrDefault();
            mainContext.PersonId = user.Id;
            try
            {
                ExecuteMe();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}