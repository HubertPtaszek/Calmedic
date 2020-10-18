using System;

namespace Calmedic.Application
{
    public class BussinesException : Exception
    {
        public BussinesException(int number, string message) : base(string.Format("({0}) {1}", number.ToString(), message))
        {
            Number = number;
            ExceptionMessage = message;
        }

        public BussinesException(string message) : base(string.Format(message))
        {
            ExceptionMessage = message;
        }

        public int Number { get; set; }
        public string ExceptionMessage { get; set; }
    }
}
