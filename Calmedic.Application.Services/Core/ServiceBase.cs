using Calmedic.Utils;
using NLog;
using System;
using System.Diagnostics;

namespace Calmedic.Application
{
    public abstract class ServiceBase : IService
    {
        public MainContext MainContext { get; set; }

        public DbSession DbSession { get; set; }

        protected void ThrowIfNull(object obj, Exception exception)
        {
            if (obj == null)
            {
                throw exception;
            }
        }

        private Logger logger = LogManager.GetCurrentClassLogger();

        protected void LogInfo(string msg)
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);
            logger.Info(string.Format(Loggers.LogFormat, DateTime.Now.ToDateTimeStringSafe(), this.GetType().Name, sf.GetMethod().Name, msg));
        }

        protected void LogError(string msg, Exception ex)
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);
            logger.Error(ex, string.Format(Loggers.LogFormat, DateTime.Now.ToDateTimeStringSafe(), this.GetType().Name, sf.GetMethod().Name, msg));
        }
    }
}
