namespace Calmedic.Application
{
    public interface IAppIdentityUserService : IService
    {
        bool IsAdmin(string identityUserId);
        bool IsDoctor(string identityUserId);
        bool IsReception(string identityUserId);
    }
}
