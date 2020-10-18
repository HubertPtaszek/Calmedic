using Newtonsoft.Json.Linq;
using Calmedic.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;

namespace Calmedic.Data
{
    public static class QueryHelpers
    {
        public static IQueryable<TEntity> PageByOptions<TEntity>(this IQueryable<TEntity> query, DxGridParams gridParams)
        {
            if (gridParams.Skip.HasValue)
            {
                query = query
                    .Skip(gridParams.Skip.Value)
                    .Take(gridParams.Take.Value);
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
                    //query = query.OrderBy(columnName);
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


        public static IQueryable<TEntity> FilterByOptions<TEntity>(this IQueryable<TEntity> query, DxGridParams gridParams)
        {
            if (!gridParams.Filter.IsNullOrEmpty())
            {
                Type elementType = query.ElementType;
                var filterTree = JArray.Parse(gridParams.Filter);
                StringBuilder sb = new StringBuilder();
                sb = ReadExpression(sb, filterTree, elementType);
                string filter = sb.ToString();
                query = DynamicQueryableExtensions.Where(query, filter);
            }
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
                DateTime outPut;
                if (DateTime.TryParse(value, out outPut))
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
    }
}
