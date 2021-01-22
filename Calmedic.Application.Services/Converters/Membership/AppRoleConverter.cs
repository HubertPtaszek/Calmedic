using Calmedic.Domain;

namespace Calmedic.Application
{
    public class AppRoleConverter : ConverterBase
    {
        public AppRoleDetailsVM ToAppRoleDetailsVM(AppRole appRole)
        {
            AppRoleDetailsVM model = new AppRoleDetailsVM()
            {
                Id = appRole.Id,
                AppRoleType = appRole.AppRoleType,
                Name = appRole.Name,
                Description = appRole.Description
            };
            return model;
        }
    }
}