using Calmedic.Dictionaries;
using System;

namespace Calmedic.Domain
{
    public class AppMailMessage : AuditEntity
    {
        public AppMailMessage()
        {
        }

        public string From { get; set; }
        public string To { get; set; }
        public string ReplyTo { get; set; }
        public string CarbonCopy { get; set; }
        public string BlindCarbonCopy { get; set; }
        public bool IsBodyHtml { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public AppMailStatus Status { get; set; }
        public EmailType EmailType { get; set; }
        public DateTime? SendDate { get; set; }
    }
}