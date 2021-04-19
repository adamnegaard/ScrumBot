using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using ScrumBot.Models.Sprint;
using ScrumBot.Models.Task;

namespace ScrumBot.Commands
{
    [Description("Commands related to user tasks")]
    public class TaskCommands : BaseCommandModule
    {
        private readonly ISprintRepository _sprintRepository;
        private readonly ITaskRepository _taskRepository;

        public TaskCommands(ISprintRepository sprintRepository, ITaskRepository taskRepository)
        {
            _sprintRepository = sprintRepository;
            _taskRepository = taskRepository;
        }

        [Command("begintask")]
        [Description("Begins a task with a list of members and a name")]
        public async Task BeginTask(CommandContext ctx, [Description("Name the user task")] [RemainingText]
            string taskName)
        {
            var currSprint = await _sprintRepository.Current(ctx.Guild.Id);
            if (currSprint == null)
            {
                await ctx.Channel.SendMessageAsync(
                    $"Currently no active sprints. Start one with the `?{nameof(SprintCommands.BeginSprint).ToLower()}` command");
                return;
            }

            if (taskName == null)
            {
                await ctx.Channel.SendMessageAsync(
                    $"Please specify a name of the task like so: `?{nameof(BeginTask).ToLower()} <{nameof(taskName).ToLower()}>`");
                return;
            }

            var taskRole = await CreateRoleFromTask(ctx, taskName);

            var task = new TaskDTO
            {
                TaskName = taskName,
                SprintId = currSprint.Id,
                StartTime = DateTime.Now,
                DiscordRoleId = taskRole.Id
            };
            await _taskRepository.Create(task);

            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = "Started a task",
                Description = $"{task.TaskName} is added to the sprint: {currSprint.SprintName}\n" +
                              $"Use the role {taskRole.Mention} to work on it.",
                Timestamp = task.StartTime
            };

            //Grant the role to the member who queried
            await ctx.Member.GrantRoleAsync(taskRole);

            //send the confirmation
            await ctx.Channel.SendMessageAsync(embedBuilder);

            //Ping everyone online
            await ctx.Channel.SendMessageAsync(
                $"@here: Work has begun on {task.TaskName}. Use the `?{nameof(JoinTask).ToLower()}`{taskRole.Mention} command to join");
        }

        [Command("endtask")]
        [Description("Ends a task by a role")]
        public async Task EndTask(CommandContext ctx, [Description("Role of the task")] DiscordRole role)
        {
            if (role == null)
            {
                await ctx.Channel.SendMessageAsync(
                    $"Please specify the role of the task like so: `?{nameof(EndTask).ToLower()} (@mention)<{nameof(role).ToLower()}>`");
                return;
            }

            var task = await _taskRepository.Read(role);
            if (task == null)
            {
                await ctx.Channel.SendMessageAsync($"Could not find the task with role {role.Mention}");
                return;
            }

            if (task.StartTime == DateTime.MinValue)
            {
                await ctx.Channel.SendMessageAsync(
                    $"This task with has not begun yet. Use the `?{nameof(BeginTask).ToLower()}` command to start a task");
                return;
            }

            if (task.EndTime != DateTime.MinValue)
            {
                await ctx.Channel.SendMessageAsync("The task has already ended");
                return;
            }

            //Start the work time on it
            task.EndTime = DateTime.Now;
            //Update it
            await _taskRepository.Update(task);

            await role.DeleteAsync("finished task");

            var timeTaken = (task.EndTime - task.StartTime).Duration();

            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = "Ended a task",
                Description =
                    $"Time taken for {task.TaskName} was: {string.Format("{0:00}:{1:00}:{2:00}", timeTaken.Hours, timeTaken.Minutes, timeTaken.Seconds)}",
                Timestamp = task.EndTime
            };
            await ctx.Channel.SendMessageAsync(embedBuilder);
        }

        [Command("jointask")]
        [Description("join a task by a role")]
        public async Task JoinTask(CommandContext ctx, [Description("Role of the task")] DiscordRole role)
        {
            if (role == null)
            {
                await ctx.Channel.SendMessageAsync(
                    $"Please specify the role of the task like so: `?{nameof(JoinTask).ToLower()} (@mention)<{nameof(role).ToLower()}>`");
                return;
            }

            var task = await _taskRepository.Read(role);
            if (task == null)
            {
                await ctx.Channel.SendMessageAsync($"Could not find the task with role {role.Mention}");
                return;
            }

            if (task.StartTime == DateTime.MinValue)
            {
                await ctx.Channel.SendMessageAsync(
                    $"This task with has not begun yet. Use the `?{nameof(BeginTask).ToLower()}` command to start a task");
                return;
            }

            if (task.EndTime != DateTime.MinValue)
            {
                await ctx.Channel.SendMessageAsync("The task has already ended");
                return;
            }

            await ctx.Member.GrantRoleAsync(role);

            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = "User joined a started task",
                Description = $"{ctx.Member.Mention} joined working on {role.Mention}",
                Timestamp = DateTime.Now
            };
            await ctx.Channel.SendMessageAsync(embedBuilder);
        }

        private async Task<DiscordRole> CreateRoleFromTask(CommandContext ctx, string taskName)
        {
            return await ctx.Guild.CreateRoleAsync(taskName, null, RandomColor());
        }

        private DiscordColor RandomColor()
        {
            var rnd = new Random();
            var val = rnd.Next(9);
            switch (val)
            {
                case 0:
                    return DiscordColor.Aquamarine;
                case 1:
                    return DiscordColor.CornflowerBlue;
                case 2:
                    return DiscordColor.Chartreuse;
                case 3:
                    return DiscordColor.Red;
                case 4:
                    return DiscordColor.Green;
                case 5:
                    return DiscordColor.Cyan;
                case 6:
                    return DiscordColor.Goldenrod;
                case 7:
                    return DiscordColor.Purple;
                case 8:
                    return DiscordColor.IndianRed;
                default:
                    return DiscordColor.Black;
            }
        }

        [Command("tasks")]
        [Description("Get a list of all tasks")]
        public async Task Tasks(CommandContext ctx)
        {
            var currSprint = await _sprintRepository.Current(ctx.Guild.Id);
            if (currSprint == null)
            {
                await ctx.Channel.SendMessageAsync(
                    $"Currently no active sprints. Start one with the `?{nameof(SprintCommands.BeginSprint).ToLower()}` command");
                return;
            }

            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = $"{currSprint.SprintName} Tasks"
            };
            foreach (var task in await _taskRepository.Read(currSprint.Tasks))
                embedBuilder.Description += $"{task.TaskName} " +
                                            (task.EndTime != DateTime.MinValue ? "**done**" : "**in progress**") + "\n";
            await ctx.Channel.SendMessageAsync(embedBuilder);
        }
    }
}