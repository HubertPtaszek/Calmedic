using System.Collections.Generic;

namespace Calmedic.Domain
{
    public class AppUser : Person
    {
        public AppUser()
        {
            UserRoles = new List<AppUserRole>();
        }
        public string AppIdentityUserId { get; set; }

        public virtual List<AppUserRole> UserRoles { get; set; }

    }
}
