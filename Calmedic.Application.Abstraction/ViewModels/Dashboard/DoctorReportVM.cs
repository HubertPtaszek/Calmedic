using Calmedic.Dictionaries;
using Calmedic.Utils;
using System;
using System.Collections.Generic;

namespace Calmedic.Application
{
    public class DoctorReportVM
    {
        public DoctorReportVM()
        {
        }

        public string DoctorName { get; set; }
        public int VisitsCount { get; set; }
    }
}