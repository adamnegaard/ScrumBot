using System;

namespace ScrumBot.Models.Reminder
{
    public class ReminderDTO
    {
        public DateTime StartTime { get; set; }
        public ulong GuildId { get; set; }
        public string Summary { get; set; }
        public bool Reminded { get; set; }
    }
}