using System;

namespace Calmedic.Data
{
    public class VisitDTO
    {
        public VisitDTO()
        { }

        public int Id { get; set; }
        public int DoctorId { get; set; }
        public bool IsLocal { get; set; }
        public string CarRegistrationNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DurationInMinutes { get; set; }
        public int? StatusId { get; set; }
        public string StatusName { get; set; }
        public string StatusColor { get; set; }
        public int BranchId { get; set; }
    }
}
