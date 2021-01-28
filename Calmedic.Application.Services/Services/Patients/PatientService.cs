using Calmedic.Data;
using Calmedic.Domain;
using Calmedic.Utils;
using DevExtreme.AspNet.Data;
using System.Linq;

namespace Calmedic.Application
{
    public class PatientService : ServiceBase, IPatientService
    {
        #region Dependencies

        public IPatientRepository PatientRepository { get; set; }
        public PatientConverter PatientConverter { get; set; }

        #endregion Dependencies

        private object _lock = new object();

        public virtual object GetPatients(DataSourceLoadOptionsBase loadOptions)
        {
            return PatientRepository.GetPatients(loadOptions);
        }

        public virtual PatientDetailsVM GetPatientDetailsVM(int id)
        {
            Patient patient = PatientRepository.GetSingle(x => x.Id == id);
            PatientDetailsVM model = PatientConverter.ToPatientDetailsVM(patient);
            return model;
        }

        public virtual int Add(PatientAddVM model)
        {
            int patientNumber = GenerateNewPatientNumber();
            Patient patient = PatientConverter.FromPatientAddVM(model, patientNumber);
            PatientRepository.Add(patient);
            PatientRepository.Save();
            return patient.Id;
        }

        private int GenerateNewPatientNumber()
        {
            lock (_lock)
            {
                int max = PatientRepository.Any()
                    ? PatientRepository.GetAll().Max(x => x.PatientNumber)
                    : NumericConstants.PatientNumberMinValue;
                if (max < NumericConstants.PatientNumberMinValue)
                    max = NumericConstants.PatientNumberMinValue;
                return max + 1;
            }
        }
    }
}