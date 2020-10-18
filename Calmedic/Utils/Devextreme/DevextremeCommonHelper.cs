using DevExtreme.AspNet.Mvc;
using DevExtreme.AspNet.Mvc.Builders;
using Calmedic.Resources.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calmedic.Utils
{
    public static class DevextremeCommonHelper
    {
        public static DateBoxBuilder DateLayoutItemSettings(this DateBoxBuilder builder, string name, DateTime? value, DateTime? maxDate = null, DateTime? minDate = null, string nullText = "", DateBoxType type = DateBoxType.Date, object htmlAttributes = null, string placeholder = "", bool readOnly = false)
        {
            return CreateDateLayoutItemSettings(builder, name, value, maxDate: maxDate, minDate: minDate, nullText: nullText, type: type, htmlAttributes: htmlAttributes, placeholder: placeholder, readOnly: readOnly);
        }

        private static DateBoxBuilder CreateDateLayoutItemSettings(DateBoxBuilder builder, string name, DateTime? value, DateTime? maxDate = null, DateTime? minDate = null, string nullText = "", string changeFunction = "", bool readOnly = false, bool setTodayAsValue = false, DateBoxType type = DateBoxType.Date, object htmlAttributes = null, string placeholder = "")
        {
            minDate = minDate ?? new DateTime(1900, 1, 1);
            maxDate = maxDate ?? new DateTime(2199, 12, 31);

            string outRangeMassage = string.Format("Zasiêg", "", minDate.ToDateStringSafe(), maxDate.ToDateStringSafe());

            builder.Name(name)
             .ID(name)
             .ReadOnly(readOnly)
             .Placeholder(placeholder)
             .ValidationMessageMode(DevExtreme.AspNet.Mvc.ValidationMessageMode.Auto)
             .Type(type);

            if (type == DateBoxType.Date)
            {
                builder.DisplayFormat(DateTimeFormats.DateFormat)
             .Min(minDate.Value)
             .Max(maxDate.Value)
             .DateOutOfRangeMessage(outRangeMassage);
            }
            else if (type == DateBoxType.Time)
            {
                builder.DisplayFormat(DateTimeFormats.ShortTimeFormat);
            }

            if (!changeFunction.IsNullOrEmpty())
                builder.OnChange(changeFunction);

            if (value.HasValue)

            {
                builder.Value(value.Value);
            }
            else if (setTodayAsValue)
            {
                builder.Value(DateTime.Today);
            }
            return builder;
        }

        public static NumberBoxBuilder NumberLayoutItemSettings(this NumberBoxBuilder builder, string name, string value, string classes = "", string placeholder = "", int? maxValue = null, int? minValue = null, string nullText = "", string changeFunction = "", NumberBoxMode mode = NumberBoxMode.Text)
        {
            return CreateNumberLayoutItemSettings(builder, name, value, classes: classes, placeholder: placeholder, maxValue: maxValue, minValue: minValue, nullText: nullText, changeFunction: changeFunction);
        }

        private static NumberBoxBuilder CreateNumberLayoutItemSettings(NumberBoxBuilder builder, string name, string value, string classes = "", string placeholder = "", int? maxValue = null, int? minValue = null, string nullText = "", string changeFunction = "", bool readOnly = false, NumberBoxMode mode = NumberBoxMode.Text)
        {
            builder.Name(name)

             .ID(name)
             .ReadOnly(readOnly)
             .ValidationMessageMode(DevExtreme.AspNet.Mvc.ValidationMessageMode.Always)
             .Mode(mode);


            if (!changeFunction.IsNullOrEmpty())
                builder.OnChange(changeFunction);


            int valueInt = 0;
            bool res = int.TryParse(value, out valueInt);

            if (res)
            {
                builder.Value(valueInt);
            }
            else
                builder.Value(null);
            builder.Placeholder(placeholder);
            return builder;
        }
    }
}