using Calmedic.Data;
using Calmedic.Dictionaries;
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
        public ClinicConverter ClinicConverter { get; set; }

        #endregion Dependencies

        public virtual ClinicListVM GetClinicListVM()
        {
            ClinicListVM model = new ClinicListVM();
            model.ClinicTypeList = EnumHelpers.GetEnumBinderList<ClinicType>();
            return model;
        }

        public virtual object GetClinics(DataSourceLoadOptionsBase loadOptions)
        {
            return new object();
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
            return model;
        }

        public virtual ClinicAddVM GetClinicAddVM()
        {
            ClinicAddVM model = new ClinicAddVM();
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
            //Clinic clinic = PatientConverter.FromClinicAddVM(model);
            //ClinicRepository.Add(clinic);
            //return clinic.Id;
            return 1;
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