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

        public int Id { get; set; }
        public string FirstLetter { get; set; }
        public string Color { get; set; }
        public string Name { get; set; }
        public int PatientNumber { get; set; }
    }
}