#region  usings 

using Vedaantees.Framework.Providers.Mailing.Models;

#endregion

namespace Vedaantees.Framework.Providers.Mailing
{
    public interface IEmailProvider
    {
        void Send(long senderId, EmailMessage emailMessage);
    }
}