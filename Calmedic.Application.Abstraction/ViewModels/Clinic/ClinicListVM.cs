using Calmedic.Utils;
using System.Collections.Generic;

namespace Calmedic.Application
{
    public class ClinicListVM
    {
        public ClinicListVM()
        {
        }

        public List<EnumModelBinder> ClinicTypeList { get; set; } = new List<EnumModelBinder>();
    }
}