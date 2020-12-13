using Calmedic.Dictionaries;
using Calmedic.Domain;
using Calmedic.Utils;
using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace Calmedic.Application
{
    public class AppMailMessageConverter : ConverterBase
    {
        public MailMessage ToMailMessage(AppMailMessage appMail, string emailUserName, string base64FooterImage)
        {
            MailMessage result = new MailMessage();
            appMail.BlindCarbonCopy?.Split(';').Where(x => x != string.Empty && x != null).ToList()?.ForEach(x => result.Bcc.Add(new MailAddress(x)));
            appMail.CarbonCopy?.Split(';').Where(x => x != string.Empty && x != null).ToList()?.ForEach(x => result.CC.Add(new MailAddress(x)));
            appMail.To?.Split(';').Where(x => x != string.Empty).ToList()?.ForEach(x => result.To.Add(new MailAddress(x)));
            appMail.ReplyTo?.Split(';').Where(x => x != string.Empty && x != null).ToList()?.ForEach(x => result.ReplyToList.Add(new MailAddress(x)));
            result.From = new MailAddress(emailUserName);
            result.Subject = appMail.Subject;
            result.IsBodyHtml = appMail.IsBodyHtml;

            AlternateView view = AlternateView.CreateAlternateViewFromString(appMail.Body, Encoding.UTF8, MediaTypeNames.Text.Html);
            if (appMail.Body.Contains("cid:footerImage") && !base64FooterImage.IsNullOrEmpty())
            {
                byte[] bitmapData = Convert.FromBase64String(base64FooterImage);
                MemoryStream stream = new MemoryStream(bitmapData);
                LinkedResource imageResource = new LinkedResource(stream)
                {
                    ContentId = "footerImage",
                    ContentType = new ContentType(MediaTypeNames.Image.Jpeg)
                };
                view.LinkedResources.Add(imageResource);
            }

            result.AlternateViews.Add(view);

            return result;
        }

        public AppMailMessage FromMailMessage(MailMessage mail, EmailType emailType)
        {
            AppMailMessage result = new AppMailMessage();
            result.To = string.Join(";", mail.To.Select(x => x.Address));
            result.ReplyTo = string.Join(";", mail.ReplyToList.Select(x => x.Address));
            result.IsBodyHtml = mail.IsBodyHtml;
            result.Status = AppMailStatus.New;
            result.EmailType = emailType;
            result.CarbonCopy = string.Join(";", mail.CC.Select(x => x.Address));
            result.BlindCarbonCopy = string.Join(";", mail.Bcc.Select(x => x.Address));
            result.Subject = mail.Subject;
            result.Body = mail.Body;
            return result;
        }
    }
}