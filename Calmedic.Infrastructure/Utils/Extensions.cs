using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Calmedic.Utils
{
    public static class BaseExtensions
    {
        public static bool EqualsOr(this object baseObject, params object[] objects)
        {
            if (baseObject == null)
                throw new NullReferenceException("Base object cannot be null");
            if (objects == null)
                return false;
            foreach (var ob in objects)
            {
                if (baseObject.Equals(ob))
                    return true;
            }
            return false;
        }

        public static bool IsNullOrEmpty(this string st)
        {
            return string.IsNullOrEmpty(st);
        }

        public static string ToStringSafe(this string st)
        {
            if (string.IsNullOrEmpty(st))
                return string.Empty;
            return st;
        }

        public static string ToStringSafe(this object st)
        {
            if (st == null)
                return string.Empty;
            if (string.IsNullOrEmpty(st.ToString()))
                return string.Empty;
            return st.ToString();
        }

        public static string ToDecimalSafe(this object st)
        {
            return st.ToStringSafe().Replace(",", ".");
        }

        public static TypeCode GetTypeCode(this Type type) => Type.GetTypeCode(Nullable.GetUnderlyingType(type) ?? type);

        public static string GetGuidFromUrl(this string st)
        {
            var actionGuid = string.Empty;
            if (st.Length > 36)
                actionGuid = st.Substring(st.Length - 36, 36);
            else if (st.Length == 36)
                actionGuid = st;
            return actionGuid;
        }

        private static string PropertyName<T>(Expression<Func<T>> expr)
        {
            var body = ((MemberExpression)expr.Body);
            return body.Member.Name;
        }

        public static byte[] ToByteArray(this Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public static string ToJson(this object obj)
        {
            if (obj == null)
                return null;
            return JsonConvert.SerializeObject(obj);
        }
    }

    public static class DateTimeHelpers
    {
        public static DateTime TodayUtc
        {
            get
            {
                return DateTime.Now.Date;
            }
        }

        public static PropertyInfo GetPropertyInfo(this Type type, string propertyName)
        {
            foreach (var property in type.GetProperties())
            {
                if (property.Name == propertyName)
                {
                    return property;
                }
                else if (!property.PropertyType.IsPrimitive && property.PropertyType != typeof(string))
                {
                    return GetPropertyInfo(property.PropertyType, propertyName);
                }
            }
            return null;
        }
    }

    public static class EnumHelpers
    {
        public static string GetDisplayName(this Enum value)
        {
            if (value != null)
            {
                FieldInfo fi = value.GetType().GetField(value.ToString());

                var attributes = (DisplayAttribute[])fi.GetCustomAttributes(
                    typeof(DisplayAttribute), false);

                if (attributes.Length > 0)
                    return attributes[0].GetName();
                else
                    return value.ToString();
            }
            return null;

        }

        public static List<SelectModelBinder> GetSelectBinderList<T>() where T : struct
        {
            List<EnumModelBinder> enumModelBinderList = GetEnumBinderList<T>();
            return enumModelBinderList.Select(x => new SelectModelBinder
            {
                Value = x.Value,
                Text = x.Text
            }).ToList();
        }

        public static List<SelectModelBinder> GetSelectBinderList<T>(Func<T, bool> whereCondition) where T : struct
        {
            List<EnumModelBinder> enumModelBinderList = GetEnumBinderListWhere<T>(whereCondition);
            return enumModelBinderList.Select(x => new SelectModelBinder
            {
                Value = x.Value,
                Text = x.Text
            }).ToList();
        }

        public static List<EnumModelBinder> GetEnumBinderListWhere<T>(Func<T, bool> whereCondition) where T : struct
        {
            var enumType = typeof(T);
            if (!enumType.IsEnum)
                throw new ArgumentException();
            IEnumerable<T> list = Enum.GetValues(enumType).Cast<T>().Where(whereCondition);
            List<EnumModelBinder> result = GetEnumBinderList<T>(list.ToArray());
            return result.OrderBy(x => x.Text).ToList();
        }

        public static List<EnumModelBinder> GetEnumBinderList<T>() where T : struct
        {
            var enumType = typeof(T);
            if (!enumType.IsEnum)
                throw new ArgumentException();
            IEnumerable<T> list = Enum.GetValues(enumType).Cast<T>();
            List<EnumModelBinder> result = GetEnumBinderList<T>(list.ToArray());
            return result.OrderBy(x => x.Text).ToList();
        }

        public static string GetEnumBinderListJson<T>() where T : struct
        {
            List<EnumModelBinder> enumList = GetEnumBinderList<T>();
            string result = JsonConvert.SerializeObject(enumList);
            return result;
        }

        public static string GetEnumBinderListJson<T>(params T[] enums) where T : struct
        {
            List<EnumModelBinder> enumList = GetEnumBinderList<T>(enums);
            string result = JsonConvert.SerializeObject(enumList);
            return result;
        }

        public static List<EnumModelBinder> GetEnumBinderList<T>(params T[] enums) where T : struct
        {
            var enumType = typeof(T);
            if (!enumType.IsEnum)
                throw new ArgumentException();
            List<EnumModelBinder> result = new List<EnumModelBinder>();
            if (enums == null)
                return result;
            foreach (T item in enums)
            {
                EnumModelBinder modelItem = new EnumModelBinder();
                modelItem.Value = Convert.ToInt32(item);
                modelItem.Text = (item as Enum).GetDisplayName();
                result.Add(modelItem);
            }
            return result.ToList();
        }

        public static EnumModelBinder GetEnumModelBinder(this Enum value)
        {
            EnumModelBinder modelItem = new EnumModelBinder();
            modelItem.Value = Convert.ToInt32(value);
            modelItem.Text = value.GetDisplayName();
            return modelItem;
        }

        public static List<T> GetEnumList<T>() where T : struct
        {
            var enumType = typeof(T);
            if (!enumType.IsEnum)
                throw new ArgumentException();
            return Enum.GetValues(enumType).Cast<T>().ToList();
        }

        public static T GetEnum<T>(int integer) where T : struct
        {
            var enumType = typeof(T);
            if (!enumType.IsEnum)
                throw new ArgumentException();
            return (T)Enum.ToObject(typeof(T), integer);
        }
    }
}
