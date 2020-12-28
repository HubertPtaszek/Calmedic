using Calmedic.Utils;
using System.Collections.Generic;

namespace Calmedic.Application
{
    public class ClinicListVM
    {
        public ClinicListVM()
        {
        }

        public List<SelectModelBinder> ClinicTypes { get; set; } = new List<SelectModelBinder>();
    }
}