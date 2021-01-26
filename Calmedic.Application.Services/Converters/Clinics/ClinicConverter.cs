using Calmedic.Domain;

namespace Calmedic.Application
{
    public class ClinicConverter : ConverterBase
    {
        #region Dependencies

        public AddressConverter AddressConverter { get; set; }

        #endregion Dependencies

        public ClinicDetailsVM ToClinicDetailsVM(Clinic clinic)
        {
            ClinicDetailsVM model = new ClinicDetailsVM()
            {
                Id = clinic.Id,
                LogoUrl = clinic.LogoUrl,
                Name = clinic.Name,
                Guid = clinic.Guid,
                ClinicType = clinic.Type,
                Address = clinic.Address.FullAdress,
                PhoneNumber = clinic.PhoneNumber,
                Email = clinic.Email,
                OpenFrom = clinic.OpenFrom,
                OpenTo = clinic.OpenTo
            };
            return model;
        }

        public ClinicEditVM ToClinicEditVM(Clinic clinic)
        {
            ClinicEditVM model = new ClinicEditVM()
            {
                Id = clinic.Id,
                LogoUrl = clinic.LogoUrl,
                Name = clinic.Name,
                Guid = clinic.Guid,
                ClinicType = clinic.Type,
                City = clinic.Address.City,
                BuildingNo = clinic.Address.BuildingNo,
                PostalCode = clinic.Address.PostalCode,
                Street = clinic.Address.Street,
                PhoneNumber = clinic.PhoneNumber,
                Email = clinic.Email,
                OpenFrom = clinic.OpenFrom,
                OpenTo = clinic.OpenTo
            };
            return model;
        }

        public Clinic FromClinicEditVM(ClinicEditVM model, Clinic clinic)
        {
            //clinic.LogoUrl = nowy url;
            clinic.Name = model.Name;
            clinic.Type = model.ClinicType;
            clinic.PhoneNumber = model.PhoneNumber;
            clinic.Email = model.Email;
            clinic.OpenFrom = model.OpenFrom;
            clinic.OpenTo = model.OpenTo;

            clinic.Address.Street = model.Street;
            clinic.Address.BuildingNo = model.BuildingNo;
            clinic.Address.City = model.City;
            clinic.Address.PostalCode = model.PostalCode;

            clinic.Address.FullAdress = AddressConverter.GetFullAddress(clinic.Address);

            return clinic;
        }
    }
}