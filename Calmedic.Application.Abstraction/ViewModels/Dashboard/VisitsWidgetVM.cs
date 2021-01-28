using Calmedic.Dictionaries;
using Calmedic.Utils;
using System;
using System.Collections.Generic;

namespace Calmedic.Application
{
    public class VisitsWidgetVM
    {
        public VisitsWidgetVM()
        {
        }

        public int Id { get; set; }
        public string FirstLetter { get; set; }
        public string Color { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}