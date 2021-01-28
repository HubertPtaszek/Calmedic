using Calmedic.Domain;
using Calmedic.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Calmedic.Data
{
    public class VisitRepository : Repository<Visit, MainDatabaseContext>, IVisitRepository
    {
        public VisitRepository(MainDatabaseContext context) : base(context)
        { }

        public List<VisitsWidgetDTO> GetCommingVisits(int clinicId)
        {
            List<VisitsWidgetDTO> result = _dbset.Where(x => x.DateFrom >= DateTime.Now && x.ClinicId == clinicId && x.Status == Dictionaries.VisitStatus.Waiting && x.DateFrom.Date == DateTime.Today)
                .OrderByDescending(x => x.DateFrom)
                .Select(x => new VisitsWidgetDTO()
                {
                    Id = x.Id,
                    DoctorName = String.Join(" ", x.Doctor.Title, x.Doctor.Person.FirstName, x.Doctor.Person.LastName),
                    PatientName = String.Join(" ", x.Patient.FirstName, x.Patient.LastName),
                    FirstLetter = x.Patient.FirstName.Substring(0, 1),
                    From = x.DateFrom,
                    To = x.DateTo
                }).Take(3).ToList();
            return result;
        }


        public List<DoctorReportDTO> GetTopDoctorsByVisitCount(int clinicId)
        {
            List<DoctorReportDTO> result = _dbset.Where(x => x.ClinicId == clinicId)
                .GroupBy(
                    x => new { x.DoctorId, x.Doctor.Person.FirstName, x.Doctor.Person.LastName }
                )
                .OrderByDescending(x => x.Count()) 
                .Select(x => new DoctorReportDTO()
                {
                    DoctorName = String.Join(" ", x.Key.FirstName, x.Key.LastName),
                    VisitsCount = x.Count(),
                }).Take(5).ToList();
            return result;
        }

        public object GetVisits(int clinicId, DateTime? selectedDate)
        {
            if (selectedDate == null)
                selectedDate = DateTime.Today;
            DateTime start = selectedDate.Value.Date;
            DateTime end = selectedDate.Value.Date.AddDays(1).AddSeconds(-1);
            var query = _dbset.Where(x => x.ClinicId == clinicId && x.Doctor != null && x.DateFrom.Date >= start && x.DateTo <= end);
            List<VisitDTO> visits = query.Select(x => new VisitDTO()
            {
                Id = x.Id,
                DoctorId = x.DoctorId,
                PatientNumber = x.Patient.PatientNumber.ToString(),
                StartDate = x.DateFrom,
                EndDate = x.DateTo,
                Status = x.Status,
                ClinicId = x.ClinicId
            }).ToList();
            return visits;
        }
    }
}