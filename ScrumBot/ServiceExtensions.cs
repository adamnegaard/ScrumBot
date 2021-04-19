using System;
using DSharpPlus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ScrumBot
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddDiscordService(this IServiceCollection services)
        {
            services.AddSingleton(serviceProvider =>
            {
                var config = serviceProvider.GetService<ClientConfig>();
                if (config == null)
                    throw new InvalidOperationException(
                        "Add a ClientConfig to the dependencies");

                //Not being used right now, but possibility for logging settings
                var logger = serviceProvider.GetRequiredService<ILoggerFactory>();

                //Configuration for the client.
                var discordConfig = new DiscordConfiguration
                {
                    Token = config.Token,
                    TokenType = TokenType.Bot,
                    LoggerFactory = logger,
                    AutoReconnect = true,
                    MinimumLogLevel = LogLevel.Debug,
                    //Setting the intents
                    Intents = DiscordIntents.AllUnprivileged
                              | DiscordIntents.Guilds
                };

                var client = new DiscordClient(discordConfig);

                //Return the service.
                return client;
            });

            return services.AddHostedService<Bot>();
        }
    }
}