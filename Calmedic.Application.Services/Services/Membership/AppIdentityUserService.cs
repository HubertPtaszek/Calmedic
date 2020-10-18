using Calmedic.Data;

namespace Calmedic.Application
{
    public class AppIdentityUserService : ServiceBase, IAppIdentityUserService
    {
        #region Dependencies
        public IAppUserRepository AppUserRepository { get; set; }
        public IAppUserRoleRepository AppUserRoleRepository { get; set; }
        #endregion

        public AppIdentityUserService()
        { }

        public virtual bool IsAdmin(string identityUserId)
        {
            return AppUserRoleRepository.CheckIfIsAdmin(identityUserId);
        }

        public virtual bool IsDoctor(string identityUserId)
        {
            return AppUserRoleRepository.CheckIfIsDoctor(identityUserId);
        }

        public virtual bool IsReception(string identityUserId)
        {
            return AppUserRoleRepository.CheckIfIsReception(identityUserId);
        }
    }
}
