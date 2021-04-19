using System;

namespace ScrumBot.Entities
{
    public class Reminder
    {
        public DateTime StartTime { get; set; }
        public ulong GuildId { get; set; }
        public string Summary { get; set; }
        public bool Reminded { get; set; }
    }
}