using System;

namespace ScrumBot.Models.Task
{
    public class TaskDTO
    {
        public int Id { get; set; }

        public string TaskName { get; set; }

        public int SprintId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public ulong DiscordRoleId { get; set; }
    }
}