using Calmedic.Dictionaries;
using Calmedic.Domain;
using Calmedic.EntityFramework;
using System.Collections.Generic;
using System.Linq;

namespace Calmedic.Data
{
    public class AppMailMessageRepository : Repository<AppMailMessage, MainDatabaseContext>, IAppMailMessageRepository
    {
        public AppMailMessageRepository(MainDatabaseContext context)
            : base(context)
        {
        }

        public List<AppMailMessage> GetMessagesToSend(int count)
        {
            return _dbset.Where(x => x.Status == AppMailStatus.New).Take(count).ToList();
        }
    }
}