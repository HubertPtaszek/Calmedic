using Calmedic.Dictionaries;
using Calmedic.Utils;
using System.Collections.Generic;

namespace Calmedic.Application
{
    public class VisitListVM
    {
        public VisitListVM()
        {
        }

        public int ClinicId { get; set; }
        public int OpenFrom { get; set; }
        public int OpenTo { get; set; }
        public List<SelectModelBinder> Doctors { get; set; } = new List<SelectModelBinder>();
        public List<SelectModelBinder> Patients { get; set; } = new List<SelectModelBinder>();
        public List<EnumModelBinder> VisitStatuses { get; set; } = EnumHelpers.GetEnumBinderList<VisitStatus>();
    }
}