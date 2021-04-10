using Core.DbConfigurations;
using Core.Entities;
using Core.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        public SiemplifyExmDBContext _db;
        public TaskRepository(SiemplifyExmDBContext db)
        {
            _db = db;

            //This needed for creating db and tables
            db.Database.EnsureCreated();
        }
        public void AddTask(TaskEntity entity)
        {
            _db.Add(entity);
            _db.SaveChanges();
        }

        public List<TaskEntity> GetTasks(int take)
        {
            var tasks = _db.Tasks.Where(x => x.Status == Constants.TaskStatusEnum.Pending).OrderBy(x => x.CreationTime).Take(take).ToList();

            foreach (var item in tasks)
            {
                item.Status = Constants.TaskStatusEnum.InProgress;
            }
            _db.SaveChanges();
            return tasks;
        }

        public void UpdateTaskAsProcessed(string taskId, Constants.TaskStatusEnum status, string consumerId)
        {
            var dbTask = _db.Find<TaskEntity>(taskId);
            dbTask.ModificationTime = DateTime.UtcNow;
            dbTask.Status = status; 
            dbTask.ConsumerID = consumerId;
            _db.SaveChanges();
        }


        public List<TaskEntity> GetLastestTasks(List<string> customerIDs)
        {
            var dbTasks = _db.Tasks.Where(x => customerIDs.Contains(x.ConsumerID)).AsNoTracking().ToList();
            return dbTasks;
        }

        public TaskStatistics GetTaskStatistics()
        {
            var stat = new TaskStatistics();
            stat.TaskCountByStatusList = _db.Tasks.GroupBy(x => x.Status).Select(x => new TaskCountByStatus { Status = x.Key, TaskCount = x.Count() }).ToList();
            var doneTaskCount = stat.TaskCountByStatusList.FirstOrDefault(x => x.Status == Constants.TaskStatusEnum.Done)?.TaskCount;
            var erroredTaskCount = stat.TaskCountByStatusList.FirstOrDefault(x => x.Status == Constants.TaskStatusEnum.Error)?.TaskCount;
            if (doneTaskCount.HasValue && erroredTaskCount.HasValue)
            {
                var totalExecutionTime = _db.Tasks.Where(x => x.ModificationTime != null).AsEnumerable().Sum(x => (x.ModificationTime.Value - x.CreationTime).TotalMilliseconds);
                stat.AvgTaskExecutionTimeInMilisecond = totalExecutionTime / doneTaskCount.Value;
                stat.PercentOfErrors = erroredTaskCount.Value / (double)doneTaskCount.Value * 100;
            }
           return stat;
        }
    }
}
