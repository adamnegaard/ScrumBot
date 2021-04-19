using System;
using System.ComponentModel.DataAnnotations;

namespace ScrumBot.Entities
{
    public class Task
    {
        [Key] public int Id { get; set; }

        [Required] public string TaskName { get; set; }

        [Required] public int SprintId { get; set; }

        public Sprint Sprint { get; set; }

        [Required] public DateTime StartTime { get; set; }

        [Required] public DateTime EndTime { get; set; }

        public ulong DiscordRoleId { get; set; }
    }
}