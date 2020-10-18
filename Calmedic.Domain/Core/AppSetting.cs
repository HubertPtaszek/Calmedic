using Calmedic.Dictionaries;

namespace Calmedic.Domain
{
    public class AppSetting : Entity
    {
        public string Value { get; set; }
        public AppSettingEnum Type { get; set; }
    }
}
