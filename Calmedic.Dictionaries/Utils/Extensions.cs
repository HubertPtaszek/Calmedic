using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Calmedic.Utils
{
    public static class Extensions
    {
        public static string ToJson(this object ob, string dateTimeFormat = null)
        {
            if (ob == null)
                return null;
            if (dateTimeFormat == null)
            {
                dateTimeFormat = DateTimeFormats.DateFormat;
            }
            var isoConvert = new IsoDateTimeConverter();
            isoConvert.DateTimeFormat = dateTimeFormat;
            string result = JsonConvert.SerializeObject(ob, isoConvert);
            return result;
        }
    }
}
