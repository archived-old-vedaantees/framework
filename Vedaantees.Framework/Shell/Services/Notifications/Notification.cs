using System;

namespace Vedaantees.Framework.Shell.Services.Notifications
{
    public class Notification
    {
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string SentBy { get; set; }
        public string CallbackUrl { get; set; }
        public string AdditionalInfo { get; set; }
        public bool MarkRead { get; set; }
        public DateTime ReadOn { get; set; }
    }
}