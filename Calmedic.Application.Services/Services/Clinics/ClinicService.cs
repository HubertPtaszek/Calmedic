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
            ClinicDetailsVM model = new ClinicDetailsVM();
            return model;
        }

        public virtual ClinicDetailsVM GetClinicDetailsVM(int id)
        {
            ClinicDetailsVM model = new ClinicDetailsVM();
            return model;
        }

        public virtual ClinicAddVM GetClinicAddVM()
        {
            ClinicAddVM model = new ClinicAddVM();
            model.ClinicTypeList = EnumHelpers.GetEnumBinderList<ClinicType>();
            return model;
        }

        public virtual int Add(ClinicAddVM model)
        {
            //Clinic clinic = PatientConverter.FromClinicAddVM(model);
            //ClinicRepository.Add(clinic);
            //return clinic.Id;
            return 1;
        }
    }
}