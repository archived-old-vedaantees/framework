#region  usings 

using System;
using System.Collections.Generic;
using Vedaantees.Framework.Providers.Storages;

#endregion

namespace Vedaantees.Framework.Providers.Mailing.Models
{
    public class EmailMessage : IEntity<string>
    {
        public EmailMessage()
        {
            To = new List<string>();
            Cc = new List<string>();
            Attachments = new List<Attachment>();
        }

        public long SenderId { get; set; }
        public string SenderEmailId { get; set; }
        public DateTime ReceivedOn { get; set; }
        public DateTime SentOn { get; set; }
        public MailStatus Status { get; set; }
        public string LastAttemptFailureMessage { get; set; }
        public List<string> To { get; set; }
        public List<string> Cc { get; set; }
        public List<Attachment> Attachments { get; set; }
        public string Content { get; set; }
        public string Subject { get; set; }

        public string Id { get; set; }
    }
}