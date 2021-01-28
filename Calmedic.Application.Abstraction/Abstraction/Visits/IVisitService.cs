using DevExtreme.AspNet.Data;
using Calmedic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calmedic.Application
{
    public interface IVisitService : IService
    {
        object GetVisits();
        void Add();
    }
}
