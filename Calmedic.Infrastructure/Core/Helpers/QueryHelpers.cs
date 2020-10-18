using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;

namespace Calmedic.Utils
{
    public static class QueryHelpers
    {
        public static IQueryable<TEntity> PageByOptions<TEntity>(this IQueryable<TEntity> query, DxGridParams gridParams)
        {
            if (gridParams.Skip.HasValue)
            {
                int take = gridParams.Take ?? 10;
                query = query
                    .Skip(gridParams.Skip.Value)
                    .Take(take);
            }
            return query;
        }

        public static IQueryable<TEntity> PageByOptions<TEntity>(this IQueryable<TEntity> query, DxAutocompleteParams autocompleteParams)
        {
            if (autocompleteParams.Skip.HasValue)
            {
                int take = autocompleteParams.Take ?? 10;
                query = query
                    .Skip(autocompleteParams.Skip.Value)
                    .Take(take);
            }
            return query;
        }

        public static IQueryable<TEntity> SortByOptions<TEntity>(this IQueryable<TEntity> query, DxGridParams gridParams)
        {
            if (!gridParams.Sort.IsNullOrEmpty())
            {
                JArray array = JArray.Parse(gridParams.Sort);
                List<string> sortOrders = new List<string>();
                foreach (var item in array.ToList())
                {
                    var sortOptions = JObject.Parse(item.ToString());
                    var columnName = (string)sortOptions.SelectToken("selector");
                    var descending = (bool)sortOptions.SelectToken("desc");

                    if (descending)
                        columnName += " DESC";
                    sortOrders.Add(columnName);

                }
                string sortOrdersStr = string.Join(",", sortOrders);
                query = DynamicQueryableExtensions.OrderBy(query, sortOrdersStr);

            }
            else
            {
                string columnName = string.Empty;
                PropertyInfo idPropertyInfo = query.ElementType.GetProperties().FirstOrDefault(x => x.Name.ToLower() == "id");
                if (idPropertyInfo != null)
                {
                    columnName = idPropertyInfo.Name;
                }
                else
                {
                    columnName = query.ElementType.GetProperties()[0].Name;
                }
                query = DynamicQueryableExtensions.OrderBy(query, columnName);
            }
            return query;
        }

        public static IQueryable<TEntity> SortByOptions<TEntity>(this IQueryable<TEntity> query, string displayProperty)
        {
            query = DynamicQueryableExtensions.OrderBy(query, displayProperty + " DESC");
            return query;
        }

        public static IQueryable<TEntity> FilterByOptions<TEntity>(this IQueryable<TEntity> query, DxGridParams gridParams)
        {
            if (!gridParams.Filter.IsNullOrEmpty())
            {
                Type elementType = query.ElementType;
                var filterTree = JArray.Parse(gridParams.Filter);
                StringBuilder sb = new StringBuilder();
                List<object> valueList = new List<object>();
                sb = ReadParamExpression(sb, filterTree, elementType, valueList);
                for (int i = 0; i < valueList.Count; i++)
                {
                    if (valueList[i] == null)
                    {
                        sb.Replace($"@{i}", "null");
                    }
                }
                string filter = sb.ToString();
                query = query.Where(filter, valueList.ToArray());
            }
            return query;
        }

        public static IQueryable<TEntity> FilterByOptions<TEntity>(this IQueryable<TEntity> query, DxAutocompleteParams autocompleteParams, string displaProperty)
        {
            if ((!autocompleteParams?.SearchValue.IsNullOrEmpty()) ?? false)
            {               
                string filter = $"{displaProperty}.Contains(@0)";
                List<object> valueList = new List<object>() { autocompleteParams.SearchValue };
                query = query.Where(filter, valueList.ToArray());
            }
            return query;
        }

        public static IQueryable<TEntity> FilterByKey<TEntity>(this IQueryable<TEntity> query, string keyProperty, object keyValue)
        {
            string filter = $"{keyProperty} == @0";
            List<object> valueList = new List<object>() { keyValue };
            query = query.Where(filter, valueList.ToArray());

            return query;
        }

        public static StringBuilder ReadExpression(StringBuilder stringBuilder, JArray array, Type elementType)
        {
            if (array[0].Type == JTokenType.String)
            {
                string filtr = GetFilterString(array[0].ToString(),
                    array[1].ToString(),
                    array[2].ToString(),
                    elementType);
                return stringBuilder.Append(filtr);
            }
            else
            {
                for (int i = 0; i < array.Count; i++)
                {
                    if (array[i] == null)
                    {
                        stringBuilder.Append(" null ");
                        continue;
                    }
                    if (array[i].ToString() == "")
                    {
                        stringBuilder.Append(" '' ");
                        continue;
                    }
                    if (array[i].ToString().Equals("and"))
                    {
                        stringBuilder.Append(" and ");
                        continue;
                    }

                    if (array[i].ToString().Equals("or"))
                    {
                        stringBuilder.Append(" or ");
                        continue;
                    }
                    JArray innerArray = (JArray)array[i];
                    if (innerArray[0].Type == JTokenType.Array)
                    {
                        stringBuilder.Append(" ( ");
                    }

                    stringBuilder = ReadExpression(stringBuilder, (JArray)array[i], elementType);

                    if (innerArray[0].Type == JTokenType.Array)
                    {
                        stringBuilder.Append(" ) ");
                    }
                }
                return stringBuilder;
            }
        }

        public static IQueryable<TEntity> FilterQuery<TEntity>(IQueryable<TEntity> source, string ColumnName, string Clause, string Value, Type elementType)
        {
            object filterValue;
            switch (Clause)
            {
                case "=":
                    filterValue = TransformValue(ColumnName, Value, elementType);
                    source = DynamicQueryableExtensions.Where(source, String.Format("{0} == {1}", ColumnName, filterValue));
                    break;
                case "contains":
                    filterValue = TransformValue(ColumnName, Value, elementType);
                    source = DynamicQueryableExtensions.Where(source, string.Format("{0}.Contains({1})", ColumnName, filterValue));
                    break;
                case "<>":
                    source = DynamicQueryableExtensions.Where(source, string.Format("!{0}.StartsWith(\"{1}\")", ColumnName, Value));
                    break;
                case ">=":
                    filterValue = TransformValue(ColumnName, Value, elementType);
                    source = DynamicQueryableExtensions.Where(source, String.Format("{0} >= {1}", ColumnName, filterValue));
                    break;
                case "<=":
                    filterValue = TransformValue(ColumnName, Value, elementType);
                    source = DynamicQueryableExtensions.Where(source, String.Format("{0} <= {1}", ColumnName, filterValue));
                    break;
                case ">":
                    filterValue = TransformValue(ColumnName, Value, elementType);
                    source = DynamicQueryableExtensions.Where(source, String.Format("{0} > {1}", ColumnName, filterValue));
                    break;
                case "<":
                    filterValue = TransformValue(ColumnName, Value, elementType);
                    source = DynamicQueryableExtensions.Where(source, String.Format("{0} < {1}", ColumnName, filterValue));
                    break;
                default:
                    break;
            }
            return source;
        }

        public static string GetFilterString(string columnName, string Clause, string Value, Type elementType)
        {
            object filterValue;
            Type propertyType = elementType.GetProperty(columnName).PropertyType;
            switch (Clause)
            {
                case "=":
                    filterValue = TransformValue(columnName, Value, elementType);
                    if (propertyType.IsEnum)
                        return String.Format("{0} = \"{1}\"", columnName, filterValue);
                    return String.Format("{0} == {1}", columnName, filterValue);
                case "contains":
                    filterValue = TransformValue(columnName, Value, elementType);
                    return string.Format("{0}.Contains({1})", columnName, filterValue);
                case "<>":
                    return string.Format("!{0}.StartsWith(\"{1}\")", columnName, Value);
                case ">=":
                    filterValue = TransformValue(columnName, Value, elementType);
                    return String.Format("{0} >= {1}", columnName, filterValue);
                case "<=":
                    filterValue = TransformValue(columnName, Value, elementType);
                    return String.Format("{0} <= {1}", columnName, filterValue);
                case ">":
                    filterValue = TransformValue(columnName, Value, elementType);
                    return String.Format("{0} > {1}", columnName, filterValue);
                case "<":
                    filterValue = TransformValue(columnName, Value, elementType);
                    return String.Format("{0} < {1}", columnName, filterValue);
                default:
                    break;
            }
            return string.Empty;
        }

        private static object TransformValue(string columnName, string value, Type elementType)
        {
            Type propertyType = elementType.GetProperty(columnName).PropertyType;
            if (propertyType == typeof(string))
            {
                return String.Format("\"{0}\"", value);
            }
            if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
            {
                if (DateTime.TryParse(value, out DateTime outPut))
                {
                    outPut = outPut.ToLocalTime();
                    return String.Format("DateTime({0},{1},{2})", outPut.Year, outPut.Month, outPut.Day);
                }
            }
            if (propertyType.IsEnum)
            {
                Array enums = Enum.GetValues(propertyType);
                int valueInt = int.Parse(value);
                foreach (Enum item in enums)
                {
                    if (Convert.ToInt32(item) == valueInt)
                    {
                        return item.ToString();
                    }
                }
            }
            return value.Replace(",", ".");
        }

        private static object GetParamValue(JToken value, Type propertyType)
        {
            Type underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

            if (underlyingType.IsEnum)
            {
                Array enums = Enum.GetValues(underlyingType);
                foreach (Enum item in enums)
                {
                    if (Convert.ToInt32(item) == value.Value<int?>())
                        return item;
                }
            }
            TypeCode code = Type.GetTypeCode(underlyingType);

            switch (code)
            {
                case TypeCode.DBNull:
                    return DBNull.Value;
                case TypeCode.Boolean:
                    return value.Value<bool?>();
                case TypeCode.Char:
                    return value.Value<char>();
                case TypeCode.SByte:
                    return value.Value<sbyte?>();
                case TypeCode.Byte:
                    return value.Value<byte?>();
                case TypeCode.Int16:
                    return value.Value<short?>();
                case TypeCode.UInt16:
                    return value.Value<ushort?>();
                case TypeCode.Int32:
                    return value.Value<int?>();
                case TypeCode.UInt32:
                    return value.Value<uint?>();
                case TypeCode.Int64:
                    return value.Value<long?>();
                case TypeCode.UInt64:
                    return value.Value<ulong?>();
                case TypeCode.Single:
                    return value.Value<float?>();
                case TypeCode.Double:
                    return value.Value<double?>();
                case TypeCode.Decimal:
                    return value.Value<decimal?>();
                case TypeCode.DateTime:
                    DateTime? dateTimeValue = value.Value<DateTime?>();
                    if (dateTimeValue?.Kind == DateTimeKind.Utc)
                        return dateTimeValue?.ToLocalTime();
                    return dateTimeValue;
                case TypeCode.String:
                    return value.Value<string>();
                case TypeCode.Empty:
                case TypeCode.Object:
                default:
                    break;
            }
            return value.ToString();
        }

        public static StringBuilder ReadParamExpression(StringBuilder stringBuilder, JArray array, Type elementType, List<object> values)
        {
            if (array[0].Type == JTokenType.String && array[0].ToString() != "!")
            {
                Type propertyType = elementType.GetProperty(array[0].ToString()).PropertyType;
                string filter = GetParamFilterString(array[0].ToString(), array[1].ToString(), propertyType, values.Count);
                values.Add(GetParamValue(array[2], propertyType));
                return stringBuilder.Append(filter);
            }
            else
            {
                foreach (var item in array)
                {
                    switch (item.ToString())
                    {
                        case null:
                            stringBuilder.Append(" null ");
                            continue;
                        case "":
                            stringBuilder.Append(" '' ");
                            continue;
                        case "!":
                            stringBuilder.Append(" !");
                            continue;
                        case "and":
                        case "or":
                            stringBuilder.Append($" {item} ");
                            continue;
                        default:
                            break;
                    }
                    if (item.First.Type == JTokenType.Array)
                        stringBuilder.Append(" ( ");

                    stringBuilder = ReadParamExpression(stringBuilder, (JArray)item, elementType, values);

                    if (item.First.Type == JTokenType.Array)
                        stringBuilder.Append(" ) ");
                }
                return stringBuilder;
            }
        }

        public static string GetParamFilterString(string columnName, string clause, Type propertyType, int index)
        {
            switch (clause)
            {
                case "=":
                    return $"{columnName} == @{index}";
                case "contains":
                    return $"{columnName}.Contains(@{index})";
                case "startswith":
                    return $"{columnName}.StartsWith(@{index})";
                case "endswith":
                    return $"{columnName}.EndsWith(@{index})";
                case "notcontains":
                    return $"!{columnName}.Contains(@{index})";
                case "<>":
                    if (propertyType == typeof(string))
                        return $"!{columnName}.StartsWith(@{index})";
                    return $"{columnName} != @{index}";
                case ">=":
                case "<=":
                case ">":
                case "<":
                    return $"{columnName} {clause} @{index}";
                default:
                    break;
            }
            return string.Empty;
        }
    }
}
