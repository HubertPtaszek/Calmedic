using Calmedic.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calmedic.Data
{
    public interface IAppMailMessageRepository : IRepository<AppMailMessage>
    {
        List<AppMailMessage> GetMessagesToSend(int count);
    }
}
