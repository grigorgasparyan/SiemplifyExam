using Core.Entities;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    public interface ITaskRepository
    {
        void AddTask(TaskEntity entity);

        List<TaskEntity> GetTasks(int take);

        void UpdateTaskAsProcessed(string taskId, Constants.TaskStatusEnum status, string consumerId);

        List<TaskEntity> GetLastestTasks(List<string> customerIDs);

        public TaskStatistics GetTaskStatistics();
    }
}
