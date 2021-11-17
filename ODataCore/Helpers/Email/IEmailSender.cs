using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Nicepet_API.Helpers
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message, string TemplateUrl);
        Task SendEmailWithAttachmentsAsync(string email, string subject, string message, List<Attachment> attachments);

    }
}
