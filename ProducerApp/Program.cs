using Core.DbConfigurations;
using Core.Entities;
using Core.Interfaces;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace ProducerApp
{
    class Program
    {
        public static IConfigurationRoot _configuration;
        static void Main(string[] args)
        {
            var serviceProvider = ConfigureServices();
            var producer = serviceProvider.GetService<Producer>();
            while (true)
            {
                Console.WriteLine("Please input 1 for Run, 2 for Statstics and 0 for Break");
                var command = Console.ReadLine();
                if (command == "1")
                {
                    Console.WriteLine("Please input how many tasks to create");
                    var taskCount = Console.ReadLine();
                    if (int.TryParse(taskCount, out int count))
                    {
                        producer.Run(count);
                    }
                    else
                    {
                        Console.WriteLine("Please input integer number");
                    }
                }
                else if (command == "0")
                {
                    break;
                }
                else if (command == "2")
                {
                    producer.GetTaskStatistics()?.Print();
                }
            }
        }


        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json")
                .Build();

            services.AddSingleton(_configuration);

            services.AddDbContext<SiemplifyExmDBContext>(opt => opt.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);

            services.AddTransient<ITaskRepository, TaskRepository>();
            services.AddTransient<Producer>();

            return services.BuildServiceProvider();
        }
    }
}
