using Calmedic.Domain;
using Calmedic.EntityFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calmedic.Data
{
    public class AppSettingsRepository : Repository<AppSetting, MainDatabaseContext>, IAppSettingsRepository
    {
        public AppSettingsRepository(MainDatabaseContext context) : base(context)
        { }

    }
}
