using DevExtreme.AspNet.Mvc;
using DevExtreme.AspNet.Mvc.Builders;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Web;

namespace Calmedic.Utils
{
    public static class Extensions
    {
        public static string GetFullErrorMessage(this ModelStateDictionary modelState)
        {
            return string.Join(" ", modelState.SelectMany(x => x.Value.Errors).Select(x => x.ErrorMessage));
        }

        public static bool IsNullOrEmpty(this HtmlString st)
        {
            if (st == null)
                return true;
            return st == new HtmlString("");
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

        public static object GetPropertyValue(this object instance, string propertyName)
        {
            Type type = instance.GetType();
            foreach (var property in type.GetProperties())
            {
                var value = property.GetValue(instance, null);
                if (property.Name == propertyName)
                {
                    return value;
                }
                else if (!property.PropertyType.IsPrimitive && property.PropertyType != typeof(string))
                {
                    return GetPropertyValue(value, propertyName);
                }
            }
            return null;
        }

        public static string HtmlDecodeFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return  html.DisplayNameFor(expression);            
        }

        public static DateBoxBuilder DatePickerFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, DateTime? maxDate = null, DateTime? minDate = null, string placeholder = "", bool readOnly = false)
        {
            if (!(expression.Body is System.Linq.Expressions.MemberExpression))
                throw new ArgumentException("Wrong expresion");
            MemberExpression memberExpresion = expression.Body as System.Linq.Expressions.MemberExpression;
            string name = memberExpresion.Member.Name;
            TModel model = html.ViewData.Model;
            DateTime? value = expression.Compile()(model) as DateTime?;

            DateBoxBuilder builder = html.DevExtreme()
                .DateBox()
                .DateLayoutItemSettings(name, value, minDate: minDate, placeholder: placeholder, readOnly: readOnly);


            return builder;
        }

        public static NumberBoxBuilder NumberBoxFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, string placeholder = "", string classes = "", int? maxValue = null, int? minValue = null)
        {
            if (!(expression.Body is System.Linq.Expressions.MemberExpression))
                throw new ArgumentException("Wrong expresion");
            MemberExpression memberExpresion = expression.Body as System.Linq.Expressions.MemberExpression;
            string name = memberExpresion.Member.Name;
            TModel model = html.ViewData.Model;
            PropertyInfo propertyInfo = model.GetType().GetProperty(name);
            if (propertyInfo == null)
                throw new ArgumentException(string.Format("Property {0} not found", name));
            string value = propertyInfo.GetValue(model) as string;

            //int valueInt = int.TryParse(value, out valueInt);

            NumberBoxBuilder builder = html.DevExtreme()
                .NumberBox()
                .NumberLayoutItemSettings(name, value, placeholder: placeholder, classes: classes, minValue: minValue, maxValue: maxValue)
                ;

            return builder;
        }

        public static DateBoxBuilder TimePickerFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression)
        {
            if (!(expression.Body is System.Linq.Expressions.MemberExpression))
                throw new ArgumentException("Wrong expresion");
            MemberExpression memberExpresion = expression.Body as System.Linq.Expressions.MemberExpression;
            string name = memberExpresion.Member.Name;
            TModel model = html.ViewData.Model;
            PropertyInfo propertyInfo = model.GetType().GetProperty(name);
            if (propertyInfo == null)
                throw new ArgumentException(string.Format("Property {0} not found", name));
            DateTime? value = propertyInfo.GetValue(model) as DateTime?;

            DateBoxBuilder builder = html.DevExtreme()
                .DateBox()
                .DateLayoutItemSettings(name, value, type: DateBoxType.Time)
                .DeferRendering(true);


            return builder;
        }

        public static IHtmlContent AssemblyVersion(this HtmlHelper helper)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            return new HtmlString(version);
        }

        public static IHtmlContent ToHtmlStringSafe(this string ob)
        {
            if (ob == null)
                ob = string.Empty;
            ob = ob.Replace("\\", "\\\\").Replace("\n", "\\n").Replace("\r", "\\r");
            return new HtmlString(ob);
        }
        public static string ToScriptStringSafe(this string ob)
        {
            if (ob == null)
                ob = string.Empty;
            return JavaScriptEncoder.Default.Encode(ob);            
        }

        public static string ToScriptStringSafe(this object ob)
        {
            return ToScriptStringSafe(ob?.ToString());
        }

        public static string GetPropertyNameFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            MemberExpression memberExpression = expression.Body as MemberExpression;
            return memberExpression.Member.Name;
        }
    }
}