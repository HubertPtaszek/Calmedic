using Calmedic.Application;
using System;

namespace Calmedic.Jobs
{
    public class SendEmailJob : Job
    {
        public SendEmailJob(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override void ExecuteMe()
        {
            IAppMailMessageService mailMessageService = _serviceProvider.GetService(typeof(IAppMailMessageService)) as IAppMailMessageService;
            mailMessageService.SendMessageJob();
        }
    }
}