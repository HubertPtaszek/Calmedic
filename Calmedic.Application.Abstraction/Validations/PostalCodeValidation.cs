namespace Calmedic.Application
{
    static public class PostalCodeValidation
    {
        static public bool IsValidate(string postalCode)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(postalCode, @"^\d{2}-\d{3}$");
        }
    }
}
