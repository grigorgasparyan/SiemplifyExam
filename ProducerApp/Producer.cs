using Core.Entities;
using Core.Interfaces;
using Core.Models;
using System;

namespace ProducerApp
{
    public class Producer
    {
        private readonly ITaskRepository _taskRepository;
        public Producer(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }
        public void Run(int count)
        {
            try
            {
                for (int i = 0; i < count; i++)
                {
                    var task = new TaskEntity
                    {
                        Id = Guid.NewGuid().ToString(),
                        CreationTime = DateTime.UtcNow,
                        Status = Core.Constants.TaskStatusEnum.Pending,
                        TaskText = $"TestTaskText {i} {DateTime.UtcNow}",
                    };
                    _taskRepository.AddTask(task);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public TaskStatistics GetTaskStatistics()
        {
            return _taskRepository.GetTaskStatistics();
        }
    }
}
