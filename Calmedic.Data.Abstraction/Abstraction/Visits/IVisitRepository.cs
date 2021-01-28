using Calmedic.Domain;
using System;
using System.Collections.Generic;

namespace Calmedic.Data
{
    public interface IVisitRepository : IRepository<Visit>
    {
        List<VisitsWidgetDTO> GetCommingVisits(int clinicId);
        List<DoctorReportDTO> GetTopDoctorsByVisitCount(int clinicId);
        object GetVisits(int clinicId, DateTime? selectedDate);
    }
}
