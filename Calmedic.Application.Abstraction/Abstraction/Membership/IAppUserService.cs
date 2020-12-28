using DevExtreme.AspNet.Data;

namespace Calmedic.Application
{
    public interface IAppUserService : IService
    {
        object GetUsersToLookup(DataSourceLoadOptionsBase loadOptions);
        object GetUsersToList(DataSourceLoadOptionsBase loadOptions);
        AppUserDetailsVM GetAppUserDetailsVM(int userId);
        AppUserEditVM GetAppUserEditVM(int userId, AppUserEditVM model = null);
        void Edit(AppUserEditVM model, int userId);
        AppUserData GetUserDataByAppIdentityUserId(string appIdentityUserId);
        int Add(AppUserAddVM model);
        AppUserAddVM GetAppUserAddVM();
    }
}
