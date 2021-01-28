using Calmedic.Dictionaries;
using Calmedic.Utils;
using System.Collections.Generic;

namespace Calmedic.Application
{
    public class ClinicListVM
    {
        public ClinicListVM()
        {
        }

        public List<EnumModelBinder> ClinicTypes { get; set; } = EnumHelpers.GetEnumBinderList<ClinicType>();
    }
}