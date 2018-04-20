using System;

namespace Vedaantees.Framework.Shell.Services.Activities
{
    public class Activity
    {
        public ActivityType ActivityType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string LinkbackUrl { get; set; }
        public int Level { get; set; }
        public DateTime OccuredOn { get; set; }
        public string UserIdentifier { get; set; }
    }
}