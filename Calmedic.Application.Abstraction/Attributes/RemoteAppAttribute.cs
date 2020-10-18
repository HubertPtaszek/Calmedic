using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Calmedic.Application
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class RemoteAppAttribute : ValidationAttribute, IClientModelValidator
    {
        public string Area { get; set; }
        private string _action { get; set; }
        private string _controller { get; set; }
        private List<string> _parameters { get; set; }

        public RemoteAppAttribute(string action, string controller, params string[] additionalParameters)
        {
            _action = action;
            _controller = controller;
            _parameters = new List<string>();
            foreach (string param in additionalParameters)
            {
                _parameters.Add(param);
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        { }
    }
}
