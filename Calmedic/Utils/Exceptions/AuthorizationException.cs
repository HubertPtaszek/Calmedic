using System;
using Microsoft.AspNetCore.Mvc;

namespace Calmedic.Utils
{
    public class AuthorizationException : Exception
    {
        public AuthorizationException(string message) : base(string.Format("{0}", message))
        { }

        public string ExceptionMessage { get; set; }
        public RedirectToRouteResult RedirectToRouteResult { get; set; }
    }
}