using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsumerApp
{
    public class Consumer
    {
        public readonly ITaskRepository _taskRepository;
        public static object lockObj = new object();
        private string _consumerId;

        public Consumer(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
            _consumerId = $"Consumer_{Guid.NewGuid()}";
        }

        public List<TaskEntity> ReadTasks(int count)
        {
            try
            {
                lock (lockObj)
                {
                    return _taskRepository.GetTasks(count);
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task ProcessTasks(int count)
        {
            while (true)
            {
                await Task.Delay(100);
                try
                {
                    var tasks = ReadTasks(count);
                    if (tasks == null)
                        continue;
                    foreach (var item in tasks)
                    {
                        try
                        {
                            Console.WriteLine(item.TaskText);
                            _taskRepository.UpdateTaskAsProcessed(item.Id, Core.Constants.TaskStatusEnum.Done, _consumerId);
                        }
                        catch (Exception)
                        {
                            _taskRepository.UpdateTaskAsProcessed(item.Id, Core.Constants.TaskStatusEnum.Error, _consumerId);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ProcessTasks Error {ex.Message}");
                }
            }
        }
    }
}
