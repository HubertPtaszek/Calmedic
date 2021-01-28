using System;

namespace Calmedic.Data
{
    public class PatientsWidgetDTO
    {
        public PatientsWidgetDTO()
        { }

        public int Id { get; set; }
        public string FirstLetter { get; set; }
        public string Name { get; set; }
        public int PatientNumber { get; set; }
    }
}
