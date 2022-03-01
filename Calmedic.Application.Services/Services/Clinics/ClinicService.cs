using Calmedic.Data;
using Calmedic.Domain;
using Calmedic.Utils;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Http;

namespace Calmedic.Application
{
    public class ClinicService : ServiceBase, IClinicService
    {
        #region Dependencies

        public IClinicRepository ClinicRepository { get; set; }
        public IClinicUserRepository ClinicUserRepository { get; set; }
        public ISpecializationRepository SpecializationRepository { get; set; }
        public ClinicConverter ClinicConverter { get; set; }

        #endregion Dependencies

        public virtual ClinicListVM GetClinicListVM()
        {
            ClinicListVM model = new ClinicListVM();
            return model;
        }

        public virtual object GetClinics(DataSourceLoadOptionsBase loadOptions, HttpContext httpContext)
        {
            AppUserData userData = httpContext.Session.GetObject<AppUserData>("AppUserData");
            object result;
            if (userData.Roles.Contains(Dictionaries.AppRoleType.Doctor))
            {
                ClinicUser clinicUser = ClinicUserRepository.GetSingle(x => x.UserId == userData.Id);
                result = ClinicRepository.GetClinicsForUser(loadOptions, clinicUser);
            }
            result = ClinicRepository.GetClinics(loadOptions);
            return result;
        }

        public virtual object GetClinicDocotrs(DataSourceLoadOptionsBase loadOptions, int clinicId)
        {
            return ClinicRepository.GetClinicDocotrs(loadOptions, clinicId);
        }

        public virtual ClinicDetailsVM GetClinicDetailsVMForUser(HttpContext context)
        {
            AppUserData userData = context.Session.GetObject<AppUserData>("AppUserData");
            ClinicDetailsVM model = new ClinicDetailsVM();
            if (userData.ClinicId.HasValue)
            {
                Clinic clinic = ClinicRepository.GetSingle(x => x.Id == userData.ClinicId.Value);
                model = ClinicConverter.ToClinicDetailsVM(clinic);
            }
            return model;
        }

        public virtual ClinicDetailsVM GetClinicDetailsVM(int id)
        {
            Clinic clinic = ClinicRepository.GetSingle(x => x.Id == id);
            ClinicDetailsVM model = ClinicConverter.ToClinicDetailsVM(clinic);
            model.Specializations = SpecializationRepository.GetSpecializationsToSelect();
            return model;
        }

        public virtual ClinicEditVM GetClinicEditVM(int id)
        {
            Clinic clinic = ClinicRepository.GetSingle(x => x.Id == id);
            ClinicEditVM model = ClinicConverter.ToClinicEditVM(clinic);
            return model;
        }

        public virtual int Add(ClinicAddVM model)
        {
            Clinic clinic = ClinicConverter.FromClinicAddVM(model);
            ClinicRepository.Add(clinic);
            return clinic.Id;
        }

        public virtual int Edit(ClinicEditVM model)
        {
            Clinic clinic = ClinicRepository.GetSingle(x => x.Id == model.Id);
            clinic = ClinicConverter.FromClinicEditVM(model, clinic);
            ClinicRepository.Edit(clinic);
            return clinic.Id;
        }
    }
}