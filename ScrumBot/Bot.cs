using System;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ScrumBot.Commands;

namespace ScrumBot
{
    public partial class Bot : IHostedService, IDisposable
    {
        private readonly IServiceProvider _services;

        public Bot(IServiceProvider services, DiscordClient client)
        {
            _services = services;
            _client = client;

            //Get the prefix object
            var config = _services.GetService<ClientConfig>();
            if (config == null)
                throw new InvalidOperationException(
                    "Add a ClientConfig to the dependencies");

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new[] {config.Prefix},
                EnableDms = true,
                EnableMentionPrefix = true,
                Services = services
            };

            //adding hooks
            _client.Ready += OnClientReady;

            //Commands adding
            _commands = _client.UseCommandsNext(commandsConfig);
            _commands.RegisterCommands<SprintCommands>();
            _commands.RegisterCommands<TaskCommands>();
        }

        private DiscordClient _client { get; }
        private CommandsNextExtension _commands { get; }

        public void Dispose()
        {
            _client.Dispose();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _client.ConnectAsync(
                new DiscordActivity("?help", ActivityType.ListeningTo),
                UserStatus.Online).ConfigureAwait(false);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _client.DisconnectAsync().ConfigureAwait(false);
        }

        private Task OnClientReady(DiscordClient client, ReadyEventArgs eventArgs)
        {
            return Task.CompletedTask;
        }
    }
}