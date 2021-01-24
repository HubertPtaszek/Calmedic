using Calmedic.Domain;

namespace Calmedic.Application
{
    public class AppUserConverter : ConverterBase
    {
        public AppUserDetailsVM ToAppUserDetailsVM(AppUser user)
        {
            var result = new AppUserDetailsVM()
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsActive = user.IsActive,
                PhoneNumber = user.PhoneNumber,
                IdentityUserId = user.AppIdentityUserId
            };
            return result;
        }

        public AppUserEditVM ToAppUserEditVM(AppUser user)
        {
            var result = new AppUserEditVM()
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsActive = user.IsActive,
                PhoneNumber = user.PhoneNumber,
                IdentityUserId = user.AppIdentityUserId
            };
            return result;
        }

        public AppUser FromAppUserEditVM(AppUserEditVM model, AppUser user)
        {
            user.Email = model.Email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.IsActive = model.IsActive;
            return user;
        }
    }
}
