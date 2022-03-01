using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Http;

namespace Calmedic.Application
{
    public interface IClinicService : IService
    {
        ClinicListVM GetClinicListVM();

        object GetClinics(DataSourceLoadOptionsBase loadOptions, HttpContext httpContext);

        object GetClinicDocotrs(DataSourceLoadOptionsBase loadOptions, int clinicId);

        ClinicDetailsVM GetClinicDetailsVMForUser(HttpContext context);

        ClinicDetailsVM GetClinicDetailsVM(int id);

        ClinicEditVM GetClinicEditVM(int id);

        int Add(ClinicAddVM model);

        int Edit(ClinicEditVM model);
    }
}