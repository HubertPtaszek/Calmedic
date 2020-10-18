using Microsoft.AspNetCore.Identity;

namespace Calmedic.Domain
{
    public class AppIdentityUser : IdentityUser
    {
        public AppIdentityUser() : base()
        { }

        public AppIdentityUser(string userName) : base(userName)
        { }
    }
}
