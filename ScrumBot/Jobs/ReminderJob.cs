using System;
using System.Threading.Tasks;
using DSharpPlus;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using ScrumBot.ClientHandler;

namespace ScrumBot.Jobs
{
    public class ReminderJob : IJob
    {
        private readonly IServiceProvider _services;

        public ReminderJob(IServiceProvider services)
        {
            _services = services;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using (var scope = _services.CreateScope())
            {
                var clientHandler = scope.ServiceProvider.GetService<IClientHandler>();
                var client = scope.ServiceProvider.GetService<DiscordClient>();
                await clientHandler.SendMeetingReminder(client);
            }
        }
    }
}