using System;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration;
using ScrumBot.Models;
using ScrumBot.Models.Reminder;

namespace ScrumBot.ClientHandler
{
    public class ClientHandler : IClientHandler
    {
        //Repos
        private readonly IReminderRepository _reminderRepository;

        //Apikeys
        private readonly string ApiKey;
        private readonly string CalendarId;

        public ClientHandler(IReminderRepository reminderRepository, IConfiguration configuration)
        {
            _reminderRepository = reminderRepository;
            ApiKey = configuration["GoogleApi:ApiKey"];
            CalendarId = configuration["GoogleApi:CalendarId"];
        }


        public async Task SendMeetingReminder(DiscordClient client)
        {
            var discordGuild = client.Guilds.FirstOrDefault(g => g.Key == Constants.GuildId).Value;
            if (discordGuild == null)
            {
                Console.Error.WriteLine("Could not find the specified guild");
                return;
            }

            var now = DateTime.Now;

            var events = await GetFoxtrotCalendarEvents(now);
            foreach (var evt in events.Items)
            {
                var eventDateStart = evt.Start != null ? evt.Start.DateTime : null;
                if (eventDateStart == null) continue;

                var eventDateEnd = evt.End != null ? evt.End.DateTime : null;
                if (eventDateEnd == null) continue;
                
                var deltaTime = (eventDateStart - now).Value.TotalMinutes;
                if (15 >= deltaTime && deltaTime > 0.0)
                {
                    var isReminded =
                        await _reminderRepository.IsReminded(eventDateStart.Value, discordGuild.Id, evt.Summary);
                    if (isReminded.status == Status.Found || isReminded.reminded) continue;

                    var reminder = new ReminderDTO
                    {
                        StartTime = eventDateStart.Value,
                        GuildId = discordGuild.Id,
                        Summary = evt.Summary,
                        Reminded = true
                    };
                    
                    var channel = discordGuild.Channels.FirstOrDefault(c => c.Key == Constants.ReminderChannelId).Value;
                    if (channel == null)
                    {
                        Console.Error.WriteLine("Could not find the reminder channel");
                        continue;
                    }

                    var foxtrotRole =
                        discordGuild.Roles.Values.FirstOrDefault(r => r.Name.ToLower().Contains("foxtrot"));
                    
                    await _reminderRepository.Create(reminder);

                    var messageText = $" **{evt.Summary}** is today from {eventDateStart:HH:mm} - {eventDateEnd:HH:mm}. Remember to start your [time tracking](https://clockify.me/tracker).";
                    
                    var reminderEmbed = new DiscordEmbedBuilder
                    {
                        Title = evt.Summary,
                        Description = messageText,
                        Timestamp = DateTimeOffset.Now,
                        Color = DiscordColor.Aquamarine
                    };
                    
                    var messageBuilder = new DiscordMessageBuilder
                    {
                        Content    = foxtrotRole != null ? foxtrotRole.Mention : "@here",
                        Embed = reminderEmbed
                    };
                    var message = await channel.SendMessageAsync(messageBuilder);
                    await message.CreateReactionAsync(DiscordEmoji.FromName(client, ":gift:"));
                }
            }
        }

        private async Task<Events> GetFoxtrotCalendarEvents(DateTime now)
        {
            var service = new CalendarService(new BaseClientService.Initializer
            {
                ApiKey = ApiKey,
                ApplicationName = "ScrumBot"
            });

            var yesterday = now.AddDays(-1).Date;
            var tomorrow = now.AddDays(1).Date;

            var request = service.Events.List(CalendarId);
            request.TimeMin = yesterday;
            request.TimeMax = tomorrow;
            request.SingleEvents = true;
            return await request.ExecuteAsync();
        }
    }
}