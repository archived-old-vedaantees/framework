using System;
using Vedaantees.Framework.Providers.Mailing.Models;
using Vedaantees.Framework.Providers.Storages.Data;
using Vedaantees.Framework.Providers.Storages.Keys;

namespace Vedaantees.Framework.Providers.Mailing
{
    public class EmailProvider : IEmailProvider
    {
        private readonly IDocumentStore _documentStore;
        private readonly IGenerateKey _generateKey;

        public EmailProvider(IDocumentStore documentStore, IGenerateKey generateKey)
        {
            _documentStore = documentStore;
            _documentStore.SetSession("Emails");
            _generateKey = generateKey;
        }

        public void Send(long senderId, EmailMessage emailMessage)
        {
            var id = _generateKey.GetNextStringKey("Email");

            emailMessage.Id = id;
            emailMessage.Status = MailStatus.Stored;
            emailMessage.ReceivedOn = DateTime.Now;
            emailMessage.LastAttemptFailureMessage = "";
            emailMessage.SenderId = senderId;
            
            _documentStore.Store(emailMessage);
        }
    }
}
