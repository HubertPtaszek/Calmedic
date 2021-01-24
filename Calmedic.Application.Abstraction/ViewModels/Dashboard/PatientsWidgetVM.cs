using Calmedic.Dictionaries;
using Calmedic.Utils;
using System;
using System.Collections.Generic;

namespace Calmedic.Application
{
    public class PatientsWidgetVM
    {
        public PatientsWidgetVM()
        {
        }

        public string FirstLetter { get; set; }
        public string Name { get; set; }
        public int PatientNumber { get; set; }
    }
}