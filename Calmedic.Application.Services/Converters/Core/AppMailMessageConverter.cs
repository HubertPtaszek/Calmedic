using Calmedic.Dictionaries;
using Calmedic.Domain;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace Calmedic.Application
{
    public class AppMailMessageConverter : ConverterBase
    {
        public MailMessage ToMailMessage(AppMailMessage appMail, string emailUserName)
        {
            MailMessage result = new MailMessage();
            appMail.To?.Split(';').Where(x => x != string.Empty).ToList()?.ForEach(x => result.To.Add(new MailAddress(x)));
            result.From = new MailAddress(emailUserName);
            result.Subject = appMail.Subject;
            result.IsBodyHtml = appMail.IsBodyHtml;
            result.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(appMail.Body, Encoding.UTF8, MediaTypeNames.Text.Html));
            return result;
        }

        public AppMailMessage FromMailMessage(MailMessage mail, EmailType emailType)
        {
            AppMailMessage result = new AppMailMessage();
            result.To = string.Join(";", mail.To.Select(x => x.Address));
            result.IsBodyHtml = mail.IsBodyHtml;
            result.Status = AppMailStatus.New;
            result.EmailType = emailType;
            result.Subject = mail.Subject;
            result.Body = mail.Body;
            return result;
        }
    }
}