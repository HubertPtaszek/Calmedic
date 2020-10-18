using Microsoft.AspNetCore.Mvc;
using Calmedic.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Calmedic.Utils
{
    public class BussinesExceptionMvc : Calmedic.Application.BussinesException
    {
        public RedirectToRouteResult RedirectToRouteResult { get; set; }
        public BussinesExceptionMvc(int number, string message, RedirectToRouteResult redirectToRouteResult)
            : base(string.Format("({0}) {1}", number.ToString(), message))
        {
            Number = number;
            ExceptionMessage = message;
            RedirectToRouteResult = redirectToRouteResult;
        }
    }
}