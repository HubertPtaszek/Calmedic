using Calmedic.Data;
using Calmedic.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Calmedic.Application
{
    public class DashboardConverter : ConverterBase
    {
        public List<PatientsWidgetVM> ToPatientsWidgetVM(List<PatientsWidgetDTO> patients)
        {
            List<PatientsWidgetVM> model = new List<PatientsWidgetVM>();
            foreach (PatientsWidgetDTO item in patients)
            {
                model.Add(new PatientsWidgetVM()
                {
                    Id = item.Id,
                    PatientNumber = item.PatientNumber,
                    Name = item.Name,
                    FirstLetter = item.FirstLetter,
                    Color = Colors.GetRandomColor(item.FirstLetter)
                });
            }
            return model;
        }

        public List<VisitsWidgetVM> ToVisitsWidgetVM(List<VisitsWidgetDTO> visits)
        {
            List<VisitsWidgetVM> model = new List<VisitsWidgetVM>();
            foreach (VisitsWidgetDTO item in visits)
            {
                model.Add(new VisitsWidgetVM()
                {
                    Id = item.Id,
                    DoctorName = item.DoctorName,
                    PatientName = item.PatientName,
                    FirstLetter = item.FirstLetter,
                    Color = Colors.GetRandomColor(item.FirstLetter),
                    From = item.From,
                    To = item.To
                });
            }
            model = model.OrderBy(x => x.From).ToList();
            return model;
        }

        public List<DoctorReportVM> ToDoctorReportVM(List<DoctorReportDTO> doctors)
        {
            List<DoctorReportVM> model = new List<DoctorReportVM>();
            foreach (DoctorReportDTO item in doctors)
            {
                model.Add(new DoctorReportVM()
                {
                    DoctorName = item.DoctorName,
                    VisitsCount = item.VisitsCount
                });
            }
            model = model.OrderBy(x => x.DoctorName).ToList();
            return model;
        }

        public List<PatientReportVM> ToPatientReportVM(List<PatientReportDTO> patients)
        {
            List<PatientReportVM> model = new List<PatientReportVM>();
            foreach (PatientReportDTO item in patients)
            {
                model.Add(new PatientReportVM()
                {
                    Age = item.Age,
                    Count = item.Count
                });
            }
            return model;
        }

        public List<VisitReportVM> ToVisitReportVM(List<VisitReportDTO> visits)
        {
            List<VisitReportVM> model = new List<VisitReportVM>();
            foreach (VisitReportDTO item in visits)
            {
                model.Add(new VisitReportVM()
                {
                    Month = item.Month,
                    Count = item.Count
                });
            }
            return model;
        }
    }
}