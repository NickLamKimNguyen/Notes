using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Common.Utils
{
    public class EmailUtil
    {
        public static async Task SendEmail(string recipients, string subject, string body, IEnumerable<Attachment> attachments)
        {
            using (var smtpClient = new SmtpClient())
            {
                MailMessage mm = new MailMessage();
                mm.To.Add(recipients);
                mm.Subject = subject;
                mm.Body = body;

                mm.IsBodyHtml = true;

                if (attachments != null && attachments.Any())
                {
                    foreach (var attachment in attachments)
                    {
                        mm.Attachments.Add(attachment);
                    }
                }

                await smtpClient.SendMailAsync(mm);
            }
        }
    }
}
