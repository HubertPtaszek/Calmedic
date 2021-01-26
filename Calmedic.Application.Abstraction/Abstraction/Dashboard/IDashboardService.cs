using DevExtreme.AspNet.Data;
using Calmedic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Calmedic.Application
{
    public interface IDashboardService : IService
    {
        ClinicDashboardVM GetClinicDashboardVM(HttpContext httpContext);
    }
}
