using Calmedic.Dictionaries;
using System.Collections.Generic;

namespace Calmedic.Utils
{
    public class MainContext
    {
        public MainContext()
        { }

        public int PersonId { get; set; }
        public List<AppRoleType> Roles { get; set; }
    }
}
