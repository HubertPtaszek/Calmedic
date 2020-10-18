using Calmedic.Resources.Shared;
using System;

namespace Calmedic.Utils
{
    public static class SystemUsers
    {
        public const string SystemUserName = "System";
        public const string SystemUserEmail = "system@system.pl";
    }

    public class Loggers
    {
        public const string NormalLogName = "NormalLog";
        public const string ExceptionLogName = "ExceptionLog";
        public const string LogFormat = "{0} {1,-30} {2,-25} - {3}";
    }

    public static class DisplayFormats
    {
        public const string PercentValue = "{0} %";
        public const string Decimal92 = "N2";
        public const string DaysValue = "{0} dni";
        public const string DolarValue = "{0}$";
        public const string DaysLeft = "0 days left";

        public static string ToDecimal92(this decimal? number)
        {
            if (number.HasValue)
                return number.Value.ToString(Decimal92);
            return string.Empty;
        }
        public static string ToDecimal92(this decimal number, string format = "")
        {
            if (format == string.Empty)
            {
                return number.ToString(Decimal92);
            }
            return string.Format(format, number.ToString(Decimal92));
        }
    }

    #region DateTimeFormats
    public static class DateTimeFormats
    {
        public const string DateFormat = "dd.MM.yyyy";
        public const string MonthFormat = "MM.yyyy";
        public const string MonthNameFormat = "MMMM";
        public const string ShortTimeFormat = "HH:mm";
        public const string DateTimeFormat = "dd.MM.yyyy HH:mm:ss";
        public const string ShortDateTimeFormat = "dd.MM.yyyy HH:mm";
        public const string ShortDateFormat = "dd.MM.yyyy";
        public const string IsoDateTimeFormat = "o";
        public const string FileDateFormat = "yyyy-MM-dd";
        public const string FileDateTimeFormat = "yyyy-MM-dd HH:mm";
        public const string DashboardDateTimeFormat = "HH:mm, dddd dd.MM.yyyy";

        public static string ToDateStringSafe(this DateTime? date)
        {
            if (date.HasValue)
                return date.Value.ToString(DateFormat);
            return string.Empty;
        }

        public static string ToDateStringSafe(this DateTime date)
        {
            if (date != null)
                return date.ToString(DateFormat);
            return string.Empty;
        }

        public static string ToShortTimeStringSafe(this DateTime? date)
        {
            if (date.HasValue)
                return date.Value.ToString(ShortTimeFormat);
            return string.Empty;
        }

        public static string ToShortTimeStringSafe(this DateTime date)
        {
            if (date != null)
                return date.ToString(ShortTimeFormat);
            return string.Empty;
        }
        public static string ToDateTimeStringSafe(this DateTime? date)
        {
            if (date.HasValue)
                return date.Value.ToString(DateTimeFormat);
            return string.Empty;
        }

        public static string ToDateTimeStringSafe(this DateTime date)
        {
            if (date != null)
                return date.ToString(DateTimeFormat);
            return string.Empty;
        }

        public static string ToShortDateTimeStringSafe(this DateTime? date)
        {
            if (date.HasValue)
                return date.Value.ToString(ShortDateTimeFormat);
            return string.Empty;
        }

        public static string ToShortDateTimeStringSafe(this DateTime date)
        {
            if (date != null)
                return date.ToString(ShortDateTimeFormat);
            return string.Empty;
        }

        public static string ToDaysLeftStringSafe(this DateTime date)
        {
            if (date != null)
            {
                var daysLeft = date - DateTime.Now;
                return daysLeft.Days.ToString(DisplayFormats.DaysLeft);
            }
            return string.Empty;
        }

        public static string ToIsoDateTimeString(this DateTime date)
        {
            return date.ToString(IsoDateTimeFormat);
        }

        public static string ToIsoDateTimeStringSafe(this DateTime? date)
        {
            if (date != null)
                return date.Value.ToString(IsoDateTimeFormat);
            return string.Empty;
        }
        public static string ToFileDateStringSafe(this DateTime? date)
        {
            if (date != null)
                return date.Value.ToString(FileDateFormat);
            return string.Empty;
        }
        public static string ToFileDateTimeStringSafe(this DateTime? date)
        {
            if (date != null)
                return date.Value.ToString(FileDateTimeFormat);
            return string.Empty;
        }
        public static string ToDashboardDateTimeStringSafe(this DateTime date)
        {
            if (date != null)
                return date.ToString(DashboardDateTimeFormat);
            return string.Empty;
        }
        public static string ToDashboardDateTimeStringSafe(this DateTime? date)
        {
            if (date.HasValue)
                return date.Value.ToString(DashboardDateTimeFormat);
            return string.Empty;
        }

        /// <summary>
        /// Get relative time string
        /// </summary>
        /// <param name="defaultFormat">Default date format when over 7 days ago</param>
        /// <returns>Relative time string eg.: minutę temu, n godzin temu</returns>
        public static string ToRelativeTimeString(this DateTime date, string defaultFormat = DashboardDateTimeFormat)
        {
            var difference = DateTime.Now - date;
            var delta = difference.TotalSeconds;

            if (delta < 2 * 60)
                return $"{DisplayFormatResource.Minutes_One} temu";
            if (delta < 4 * 60)
                return $"{difference.Minutes} {DisplayFormatResource.Minutes_Few} temu";
            if (delta < 60 * 60)
                return $"{difference.Minutes} {DisplayFormatResource.Minutes_Many} temu";
            if (delta < 2 * 60 * 60)
                return $"{DisplayFormatResource.Hours_One} temu";
            if (delta < 5 * 60 * 60)
                return $"{difference.Hours} {DisplayFormatResource.Hours_Few} temu";
            if (delta < 24 * 60 * 60)
                return $"{difference.Hours} {DisplayFormatResource.Hours_Many} temu";
            if (delta < 48 * 60 * 60)
                return "wczoraj";
            if (delta < 7 * 24 * 60 * 60)
                return $"{difference.Days} {DisplayFormatResource.Days_Few} temu";
            return date.ToString(defaultFormat);
        }
    }
    #endregion
}
