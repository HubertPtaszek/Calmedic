using Calmedic.Dictionaries;
using System;

namespace Calmedic.Application
{
    public class VisitEditVM
    {
        public VisitEditVM()
        {
        }

        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Description { get; set; }
        public VisitStatus Status { get; set; }
        public int ClinicId { get; set; }
    }
}