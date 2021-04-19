using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using ScrumBot.Models.Sprint;

namespace ScrumBot.Commands
{
    [Description("Commands related to sprints")]
    public class SprintCommands : BaseCommandModule
    {
        private readonly ISprintRepository _sprintRepository;

        public SprintCommands(ISprintRepository sprintRepository)
        {
            _sprintRepository = sprintRepository;
        }

        [Command("beginsprint")]
        [Description("Starts a new sprint and sets it to the currently active sprint")]
        public async Task BeginSprint(CommandContext ctx, [Description("Name the sprint")] [RemainingText]
            string sprintName)
        {
            var currSprint = await _sprintRepository.Current(ctx.Guild.Id);
            if (currSprint != null)
            {
                await ctx.Channel.SendMessageAsync(
                    $"There is already a sprint active named {currSprint.SprintName}, please end it with the `?{nameof(EndSprint).ToLower()}` command");
                return;
            }

            if (sprintName == null)
            {
                await ctx.Channel.SendMessageAsync(
                    $"Please specify a name of the sprint like so: `?{nameof(BeginSprint).ToLower()} <{nameof(sprintName).ToLower()}>`");
                return;
            }

            var sprint = new SprintDTO
            {
                SprintName = sprintName,
                StartDate = DateTime.Now,
                GuildId = ctx.Guild.Id
            };
            await _sprintRepository.Create(sprint);

            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = "New Sprint Started",
                Description = $"{sprintName} has begun!",
                Timestamp = sprint.StartDate
            };
            await ctx.Channel.SendMessageAsync(embedBuilder);
        }

        [Command("endsprint")]
        [Description("Ends the currently active sprint")]
        public async Task EndSprint(CommandContext ctx)
        {
            var currSprint = await _sprintRepository.Current(ctx.Guild.Id);
            if (currSprint == null)
            {
                await ctx.Channel.SendMessageAsync(
                    $"Currently no active sprints. Start one with the `?{nameof(BeginSprint).ToLower()}` command");
                return;
            }

            currSprint.EndDate = DateTime.Now;
            currSprint.IsCurrent = false;
            await _sprintRepository.Update(currSprint);

            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = "Sprint Ended",
                Description =
                    $"{currSprint.SprintName} is over!\nLasted for {(currSprint.EndDate - currSprint.StartDate).Days}"
            };
            await ctx.Channel.SendMessageAsync(embedBuilder);
        }
    }
}