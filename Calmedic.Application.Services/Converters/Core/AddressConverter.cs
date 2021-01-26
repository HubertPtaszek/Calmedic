using Calmedic.Domain;
using Calmedic.Resources.Shared;
using Calmedic.Utils;

namespace Calmedic.Application
{
    public class AddressConverter : ConverterBase
    {
        public string GetFullAddress(Address address)
        {
            string result = ErrorResource.NoData;
            if (address != null)
            {
                string cityAndPostalCode, apartmentNo;
                apartmentNo = !address.ApartmentNo.IsNullOrEmpty() ? $"/{address.ApartmentNo}" : string.Empty;
                cityAndPostalCode = $",\n{address.PostalCode} {address.City}";

                if (!IsAddressEmpty(address))
                    result = $"{address.Street} {address.BuildingNo}{apartmentNo}{cityAndPostalCode}";
            }
            return result;
        }

        public bool IsAddressEmpty(Address address)
        {
            return ((address == null) || (address.Street.IsNullOrEmpty() && address.BuildingNo.IsNullOrEmpty()
                && address.City.IsNullOrEmpty() && address.PostalCode.IsNullOrEmpty()));
        }
    }
}