using Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Models
{
    public class TaskStatistics
    {
        public List<TaskCountByStatus> TaskCountByStatusList { get; set; }
        public double AvgTaskExecutionTimeInMilisecond { get; set; }
        public double PercentOfErrors { get; set; }

        public void Print()
        {
            var doneTaskCount = TaskCountByStatusList?.FirstOrDefault(x => x.Status == Constants.TaskStatusEnum.Done)?.TaskCount;
            Console.WriteLine($"Done {doneTaskCount.GetValueOrDefault()}");
            var erroredTaskCount = TaskCountByStatusList?.FirstOrDefault(x => x.Status == Constants.TaskStatusEnum.Error)?.TaskCount;
            Console.WriteLine($"Error {erroredTaskCount.GetValueOrDefault()}");
            var inProgressTaskCount = TaskCountByStatusList?.FirstOrDefault(x => x.Status == Constants.TaskStatusEnum.InProgress)?.TaskCount;
            Console.WriteLine($"InProgress {inProgressTaskCount.GetValueOrDefault()}");
            var pendingTaskCount = TaskCountByStatusList?.FirstOrDefault(x => x.Status == Constants.TaskStatusEnum.Pending)?.TaskCount;
            Console.WriteLine($"Pending {pendingTaskCount.GetValueOrDefault()}");

            Console.WriteLine($"Avg Task Execution Time In Milisecond {AvgTaskExecutionTimeInMilisecond}");
            Console.WriteLine($"Percent Of Errors {PercentOfErrors}%");
        }
    }

    public class TaskCountByStatus
    {
        public TaskStatusEnum Status { get; set; }
        public int TaskCount { get; set; }
    }
}
