using Calmedic.Data;
using Calmedic.Dictionaries;
using Calmedic.Domain;
using Calmedic.Resources.Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Calmedic.Application
{
    public class AppSettingsService : ServiceBase, IAppSettingsService
    {
        #region Dependencies

        public IAppSettingsRepository AppSettingsRepository { get; set; }

        #endregion Dependencies

        private IList<AppSetting> _appSettings;

        private T GetSetting<T>(AppSettingEnum type)
        {
            if (_appSettings == null)
            {
                _appSettings = AppSettingsRepository.GetAll();
            }
            AppSetting element = _appSettings.FirstOrDefault(x => x.Type == type);
            if (element == null || element.Value == null)
            {
                throw new Exception(string.Format(ErrorResource.NoSetting, type.ToString()));
            }

            return (T)Convert.ChangeType(element.Value, typeof(T), CultureInfo.InvariantCulture);
        }

        public int GetEmailPort()
        {
            return GetSetting<int>(AppSettingEnum.EmailPort);
        }

        public string GetEmailServer()
        {
            return GetSetting<string>(AppSettingEnum.EmailServer);
        }

        public string GetEmailUserName()
        {
            return GetSetting<string>(AppSettingEnum.EmailUserName);
        }

        public string GetEmailUserPassword()
        {
            return GetSetting<string>(AppSettingEnum.EmailUserPassword);
        }
    }
}