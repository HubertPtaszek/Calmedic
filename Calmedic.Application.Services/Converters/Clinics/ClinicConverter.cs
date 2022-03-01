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

        public Clinic FromClinicAddVM(ClinicAddVM model)
        {
            Clinic clinic = new Clinic()
            {
                //clinic.LogoUrl = nowy url,
                Name = model.Name,
                Type = model.ClinicType,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                OpenFrom = model.OpenFrom,
                OpenTo = model.OpenTo
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
            clinic.Address = address;
            return clinic;
        }
    }
}