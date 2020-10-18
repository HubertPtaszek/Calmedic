using DevExtreme.AspNet.Data;
using Calmedic.Domain;
using Calmedic.Utils;
using System.Collections.Generic;

namespace Calmedic.Data
{
    public interface IAppRoleRepository : IRepository<AppRole>
    {
        object GetRolesToList(DataSourceLoadOptionsBase loadOptions);
        List<SelectModelBinder<int>> GetRolesToSelect();
    }
}
