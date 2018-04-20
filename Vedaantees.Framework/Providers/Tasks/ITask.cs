using System;
using System.Threading.Tasks;
using Vedaantees.Framework.Types.Results;

namespace Vedaantees.Framework.Providers.Tasks
{
    public interface ITask
    {
        Task<MethodResult> Execute();
    }

    public class TaskAttribute : Attribute
    {
        public int IntervalInMinutes { get; set; }
        public string Name { get; set; }
        public TaskType TaskType { get; set; }
    }

    public enum TaskType
    {
        Recurring,
        BackgroundJob
    }
}