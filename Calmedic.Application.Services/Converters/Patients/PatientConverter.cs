using Calmedic.Domain;

namespace Calmedic.Application
{
    public class PatientConverter : ConverterBase
    {
        #region Dependencies

        public AddressConverter AddressConverter { get; set; }

        #endregion Dependencies

        public PatientDetailsVM ToPatientDetailsVM(Patient patient)
        {
            PatientDetailsVM model = new PatientDetailsVM()
            {
                Id = patient.Id,
                PatientNumber = patient.PatientNumber,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Pesel = patient.Pesel,
                DateOfBirth = patient.DateOfBirth,
                Sex = patient.Sex,
                City = patient.Address.City,
                Street = patient.Address.Street,
                BuildingNo = patient.Address.BuildingNo,
                ApartmentNo = patient.Address.ApartmentNo,
                PostalCode = patient.Address.PostalCode,
                EmailAddress = patient.EmailAddress,
                PhoneNumber = patient.PhoneNumber,
                Comments = patient.Comments
            };
            return model;
        }

        public Patient FromPatientAddVM(PatientAddVM model, int patientNumber)
        {
            Patient patient = new Patient()
            {
                PatientNumber = patientNumber,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Pesel = model.Pesel,
                DateOfBirth = model.DateOfBirth,
                Sex = model.Sex,
                EmailAddress = model.EmailAddress,
                PhoneNumber = model.PhoneNumber,
                Comments = model.Comments
            };

            Address address = new Address()
            {
                City = model.City,
                PostalCode = model.PostalCode,
                Street = model.Street,
                BuildingNo = model.BuildingNo,
                ApartmentNo = model.ApartmentNo
            };
            address.FullAdress = AddressConverter.GetFullAddress(address);
            patient.Address = address;
            return patient;
        }
    }
}