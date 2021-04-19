using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using ScrumBot.ClientHandler;
using ScrumBot.Entities;
using ScrumBot.Jobs;
using ScrumBot.Models.Reminder;
using ScrumBot.Models.Sprint;
using ScrumBot.Models.Task;

namespace ScrumBot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<IScrumContext, ScrumContext>(o =>
                o.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            //Database related
            services.AddScoped<ISprintRepository, SprintRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IReminderRepository, ReminderRepository>();

            //Discord related
            services.AddScoped<IClientHandler, ClientHandler.ClientHandler>();

            services.AddSingleton<ClientConfig>();


            //The discord bot
            services.AddDiscordService();

            //Quarts scheduling services
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            //The timing for our jobs gets set inside the class of QuartzHostedService
            services.AddSingleton<ReminderJob>();

            services.AddHostedService<QuartzHostedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }
    }
}