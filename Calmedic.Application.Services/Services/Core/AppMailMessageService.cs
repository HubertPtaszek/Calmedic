using Calmedic.Data;
using Calmedic.Dictionaries;
using Calmedic.Domain;
using Calmedic.Resources.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Calmedic.Application
{
    public class AppMailMessageService : ServiceBase, IAppMailMessageService
    {
        #region Dependencies

        public AppSettingsService AppSettingsService { get; set; }
        public IAppMailMessageRepository AppMailMessageRepository { get; set; }
        public AppMailMessageConverter AppMailMessageConverter { get; set; }
        public AppUserService AppUserService { get; set; }
        public IServiceProvider ServiceProvider { get; set; }
        public IAppUserRepository AppUserRepository { get; set; }

        #endregion Dependencies

        public virtual void SendMessageJob()
        {
            List<AppMailMessage> listToSend = AppMailMessageRepository.GetMessagesToSend(100);
            if (listToSend.Any())
            {
                string emailUserName = AppSettingsService.GetEmailUserName();
                foreach (AppMailMessage mail in listToSend)
                {
                    SendMessage(mail, emailUserName);
                }
            }
        }

        public virtual AppMailMessage AddAppMailMessage(MailMessage mail, EmailType emailType)
        {
            AppMailMessage message = AppMailMessageConverter.FromMailMessage(mail, emailType);
            AppMailMessageRepository.Add(message);
            AppMailMessageRepository.Save();
            return message;
        }

        private void SendMessage(AppMailMessage appMail, string emailUserName)
        {
            MailMessage mail = AppMailMessageConverter.ToMailMessage(appMail, emailUserName);
            try
            {
                SendMessage(mail);

                appMail.Status = AppMailStatus.Sent;
                appMail.From = emailUserName;
            }
            catch (Exception e)
            {
                LogError(string.Format(ErrorResource.MailMessageSending, appMail.Id.ToString()), e);
                appMail.Status = AppMailStatus.Error;
            }
            AppMailMessageRepository.Edit(appMail);
        }

        private void SendMessage(MailMessage message)
        {
            string credentailUserName = AppSettingsService.GetEmailUserName();
            string pwd = AppSettingsService.GetEmailUserPassword();
            string server = AppSettingsService.GetEmailServer();
            int port = AppSettingsService.GetEmailPort();
            SmtpClient client = new SmtpClient(server);
            client.Port = port;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            bool areCredentialsSupplied = !string.IsNullOrEmpty(pwd);
            if (areCredentialsSupplied)
            {
                client.UseDefaultCredentials = false;
                NetworkCredential credentails = new NetworkCredential(credentailUserName, pwd);
                client.EnableSsl = true;
                client.Credentials = credentails;
            }
            else
            {
                client.UseDefaultCredentials = true;
                client.EnableSsl = false;
            }
            client.Send(message);
        }

        public bool AddForgotPasswordMessage(string url, string userId)
        {
            UserManager<AppIdentityUser> userManager = ServiceProvider.GetService(typeof(UserManager<AppIdentityUser>)) as UserManager<AppIdentityUser>;
            AppIdentityUser user = userManager.FindByIdAsync(userId).Result;
            var code = userManager.GeneratePasswordResetTokenAsync(user).Result;
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            string callbackUrl = url.Replace("__code__", code);
            MailMessage mail = new MailMessage();
            mail.To.Add(user.Email);
            mail.Subject = ErrorResource.ForgotPasswordSubject;
            mail.Body = string.Format(ErrorResource.ForgotPasswordContent, callbackUrl);
            mail.IsBodyHtml = true;

            AddAppMailMessage(mail, EmailType.ForgotPassword);
            return true;
        }

        public virtual bool AddCreateConfirmationMessage(int id, string url)
        {
            AppUser appUser = AppUserRepository.GetSingle(x => x.Id == id);
            if (appUser == null)
            {
                return false;
            }
            UserManager<AppIdentityUser> userManager = ServiceProvider.GetService(typeof(UserManager<AppIdentityUser>)) as UserManager<AppIdentityUser>;
            AppIdentityUser user = userManager.FindByIdAsync(appUser.AppIdentityUserId).Result;
            var code = userManager.GenerateEmailConfirmationTokenAsync(user).Result;
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            string callbackUrl = url.Replace("__code__", code).Replace("__userId__", user.Id);
            MailMessage mail = new MailMessage();
            mail.To.Add(appUser.Email);
            mail.Subject = ErrorResource.ConfirmationMessageSubject;
            mail.Body = string.Format(ErrorResource.ConfirmationMessageContent, callbackUrl);
            mail.IsBodyHtml = true;

            AddAppMailMessage(mail, EmailType.CreateAccount);
            return true;
        }
    }
}