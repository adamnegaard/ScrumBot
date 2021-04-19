using System;
using System.Collections.Generic;

namespace ScrumBot.Models.Sprint
{
    public class SprintDTO
    {
        public int Id { get; set; }
        public string SprintName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsCurrent { get; set; }

        public ulong GuildId { get; set; }

        public IEnumerable<int> Tasks { get; set; }
    }
}