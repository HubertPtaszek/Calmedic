using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Calmedic.Utils
{
    public class RoleAuthorizationException : Exception
    {
        public string ExceptionMessage { get; set; }
        public RedirectToRouteResult RedirectToRouteResult { get; set; }
        public RoleAuthorizationException(string message)
            : base(string.Format("{0}", message))
        {

        }
    }
}