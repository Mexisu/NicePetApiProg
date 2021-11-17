using MailKit.Security;
using Microsoft.Extensions.Options;
using Nicepet_API.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Nicepet_API.Helpers;
using Nicepet_API.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Nicepet_API.Helpers
{
    public class HtmlOutputFormatter : StringOutputFormatter
    {
        public HtmlOutputFormatter()
        {
            SupportedMediaTypes.Add("text/html");
        }
    }

    public class EmailSender : IEmailSender
    {
        public static IWebHostEnvironment _environment;
        //private ApiNicepetContext _db;
        public EmailAppSettings _emailSettings { get; }

        public EmailSender(IOptions<EmailAppSettings> emailSettings, IWebHostEnvironment environment)
        {
            _emailSettings = emailSettings.Value;
            _environment = environment;
        }

        public Task SendEmailAsync(string email, string subject, string message, string TemplateUrl)
        {
            Execute(email, subject, message, null, TemplateUrl).Wait();

            return Task.FromResult(0);
        }

        public Task SendEmailWithAttachmentsAsync(string email, string subject, string message, List<Attachment> attachments)
        {
            Execute(email, subject, message, attachments, null).Wait();

            return Task.FromResult(0);
        }

        public async Task<string> Execute(string email, string subject, string message, List<Attachment> attachments, string TemplateUrl)
        {
            try
            {
                var toEmail = string.IsNullOrEmpty(email) ? _emailSettings.ToEmail : email;

                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(_emailSettings.FromAddress, _emailSettings.FromName)
                };

                mail.To.Add(new MailAddress(toEmail));

                if (!string.IsNullOrEmpty(_emailSettings.CcEmail))
                    mail.CC.Add(new MailAddress(_emailSettings.CcEmail));

                if (!string.IsNullOrEmpty(_emailSettings.BccEmail))
                    mail.Bcc.Add(new MailAddress(_emailSettings.BccEmail));

                if (attachments != null)
                {
                    foreach (var item in attachments)
                    {
                        mail.Attachments.Add(item);
                    }
                }
                mail.Subject = subject;

                string body = string.Empty;
                mail.IsBodyHtml = true;
                mail.Body = message;
               
                if (!String.IsNullOrEmpty(TemplateUrl))
                {
                    if (TemplateUrl.IndexOf(".html") > -1)
                    {

                        using (StreamReader reader = new StreamReader(_environment.WebRootPath + "/EmailTemplates/" + TemplateUrl))
                        {
                            body = reader.ReadToEnd();
                        }
                        mail.Body = body;

                    }
                }
                mail.Priority = MailPriority.Normal;

                using (SmtpClient smtp = new SmtpClient(_emailSettings.ServerAddress, _emailSettings.ServerPort))
                {
                    smtp.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);
                    smtp.EnableSsl = _emailSettings.ServerUseSsl;
                    await smtp.SendMailAsync(mail);

                }
                return "ok";

            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

    }
}
