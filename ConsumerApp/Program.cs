using Core.DbConfigurations;
using Core.Interfaces;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ConsumerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = ConfigureServices();
            Console.WriteLine("How many consumers to run?");
            var consumersCountString = Console.ReadLine();
            Console.WriteLine("How many task should process each one?");
            var taskCountString = Console.ReadLine();
            if (int.TryParse(consumersCountString, out int consumersCount) && int.TryParse(taskCountString, out int taskCount))
            {
                List<Task> tasks = new List<Task>();
                for (int i = 0; i < consumersCount; i++)
                {
                    var consumer = serviceProvider.GetService<Consumer>();
                    tasks.Add(Task.Run(() => consumer.ProcessTasks(taskCount)));
                }
            }
            Console.ReadLine();
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json")
                .Build();

            services.AddSingleton(configuration);

            services.AddDbContext<SiemplifyExmDBContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);

            services.AddTransient<ITaskRepository, TaskRepository>();
            services.AddTransient<Consumer>();

            return services.BuildServiceProvider();
        }
    }
}
