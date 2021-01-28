using System;

namespace Calmedic.Data
{
    public class VisitsWidgetDTO
    {
        public VisitsWidgetDTO()
        { }

        public int Id { get; set; }
        public string FirstLetter { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}