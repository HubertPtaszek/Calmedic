using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Http;

namespace Calmedic.Application
{
    public interface IClinicService : IService
    {
        ClinicListVM GetClinicListVM();
        object GetClinics(DataSourceLoadOptionsBase loadOptions);
        ClinicDetailsVM GetClinicDetailsVMForUser(HttpContext context);
        ClinicDetailsVM GetClinicDetailsVM(int id);
        ClinicEditVM GetClinicEditVM(int id);
        ClinicAddVM GetClinicAddVM();
        int Add(ClinicAddVM model);
        int Edit(ClinicEditVM model);

    }
}
