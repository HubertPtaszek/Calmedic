using System.ComponentModel.DataAnnotations;

namespace Calmedic.Application
{
    public class PostalCodeAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            return PostalCodeValidation.IsValidate(value.ToString());
        }
    }
}
