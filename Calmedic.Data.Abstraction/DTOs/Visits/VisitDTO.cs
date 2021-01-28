using Calmedic.Dictionaries;
using System;

namespace Calmedic.Data
{
    public class VisitDTO
    {
        public VisitDTO()
        { }

        public int Id { get; set; }
        public int DoctorId { get; set; }
        public string PatientNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DurationInMinutes
        {
            get
            {
                return (int)(EndDate.TimeOfDay - StartDate.TimeOfDay).TotalMinutes;
            }
        }
        public VisitStatus Status { get; set; }
        public string StatusColor
        {
            get
            {
                switch (Status) {
                    case VisitStatus.Waiting:
                        return "#1E90FF";
                    case VisitStatus.Delayed:
                        return "#FF8817";
                    case VisitStatus.InProgress:
                        return "#AE7FCC";
                    case VisitStatus.Canceled:
                        return "#E18E92";
                    case VisitStatus.Finished:
                        return "#56CA85";
                    default:
                        return "#1E90FF";
                }
            }
        }
        public int ClinicId { get; set; }
    }
}
