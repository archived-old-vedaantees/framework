using System.Collections.Generic;
using System.Linq;
using Hangfire;

namespace Vedaantees.Framework.Providers.Tasks
{
    public class TasksManager
    {
        private readonly IEnumerable<ITask> _tasks;

        public TasksManager(IEnumerable<ITask> tasks)
        {
            _tasks = tasks;
        }

        public void Configure()
        {
            foreach (var task in _tasks)
            {
                var attributes = task.GetType().GetCustomAttributes(typeof(TaskAttribute), true).FirstOrDefault() as TaskAttribute;

                if(attributes?.TaskType==TaskType.Recurring)
                    RecurringJob.AddOrUpdate<ITask>(attributes.Name, x=>x.Execute(), Cron.MinuteInterval(attributes.IntervalInMinutes));
            }

            var server = new BackgroundJobServer();
        }
    }
}
