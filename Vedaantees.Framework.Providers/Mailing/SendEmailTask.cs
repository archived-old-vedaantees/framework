#region " usings "

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using Vedaantees.Framework.Providers.Logging;
using Vedaantees.Framework.Providers.Mailing.Models;
using Vedaantees.Framework.Providers.Storages.Data;
using Vedaantees.Framework.Providers.Tasks;
using Vedaantees.Framework.Types.Results;

#endregion

namespace Vedaantees.Framework.Providers.Mailing
{
    [Task(Name = "SendEmail", IntervalInMinutes = 5,TaskType = TaskType.Recurring)]
    public class SendEmailTask : ITask
    {
        private readonly IDocumentStore _dataRepository;
        private readonly EmailSettings _emailSettings;
        private readonly ILogger _logger;

        public SendEmailTask(IDocumentStore dataRepository, EmailSettings emailSettings, ILogger logger)
        {
            _dataRepository = dataRepository;
            _dataRepository.SetSession("Emails");
            _emailSettings = emailSettings;
            _logger = logger;
            
            IsSingleInstance = true;
        }

        public async Task<MethodResult> Execute()
        {
            Console.WriteLine(@"Executing the Send Email Task");

            var emails = _dataRepository.Find<EmailMessage>(p => p.Status == MailStatus.NotSentSuccessfully || p.Status == MailStatus.Stored && p.To.Count != 0);

            if (emails==null)
                return new MethodResult(MethodResultStates.Successful);

            foreach (var email in emails)
            {
                var actionResult = await Task.Run(()=> Send(ToMailMessage(email), email.SenderEmailId));

                if (actionResult.MethodResultState == MethodResultStates.Successful)
                {
                    Console.WriteLine(@"Sent email to " + email.To);
                    email.SentOn = DateTime.Now;
                    email.Status = MailStatus.SentSuccessfully;
                }
                else
                {
                    email.LastAttemptFailureMessage = actionResult.Message;
                    Console.WriteLine(@"Error sending email " + email.LastAttemptFailureMessage);
                }

                _dataRepository.Store(email);
            }

            Console.WriteLine(@"Done the Send Email Task");
            return new MethodResult(MethodResultStates.Successful);
        }

        private MailMessage ToMailMessage(EmailMessage emailMessage)
        {
            var inlines = new List<LinkedResource>();
            var avHtml = AlternateView.CreateAlternateViewFromString(emailMessage.Content, null, MediaTypeNames.Text.Html);
            var mail = new MailMessage();

            //foreach (var attachment in emailMessage.Attachments.Where(p => p.IsEmbedded))
            //{
            //    inlines.Add(new LinkedResource(attachment.Path, attachment.MediaType) { ContentId = Guid.NewGuid().ToString() });
            //    var att = new System.Net.Mail.Attachment(_filesStore.Download(attachment.Path).Result, attachment.Name, attachment.MediaType);
            //    att.ContentDisposition.Inline = true;
            //    mail.Attachments.Add(att);
            //}

            //avHtml.LinkedResources.AddRange(inlines);
            mail.AlternateViews.Add(avHtml);
            
            foreach (var emailAddress in emailMessage.To)
                mail.To.Add(emailAddress);

            foreach (var emailAddress in emailMessage.Cc)
                mail.To.Add(emailAddress);

            mail.Subject = emailMessage.Subject;
            mail.IsBodyHtml = true;
            return mail;
        }

        private MethodResult Send(MailMessage mailMessage, string from)
        {
            try
            {
                var smtpClient = new SmtpClient(_emailSettings.Server, Convert.ToInt32(_emailSettings.Port))
                {
                    Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password),
                    EnableSsl = true
                };

                mailMessage.From = new MailAddress(from);
                mailMessage.IsBodyHtml = true;
                smtpClient.Send(mailMessage);

                return new MethodResult(MethodResultStates.Successful, "Email sent successfully");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while sending email.");
                return new MethodResult(ex);
            }
        }

        public bool IsSingleInstance { get; set; }
    }
}