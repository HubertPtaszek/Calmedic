using System.Collections.Generic;

namespace Calmedic.Data
{
    public class SchedulerVisitDTO
    {
        public List<VisitDTO> Visits { get; set; }
        public List<DoctorDTO> Doctors { get; set; }
    }
}
