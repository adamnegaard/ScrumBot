using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ScrumBot.Entities
{
    public class Sprint
    {
        [Key] public int Id { get; set; }

        [Required] public string SprintName { get; set; }

        [Required] public DateTime StartDate { get; set; }

        [Required] public DateTime EndDate { get; set; }

        public bool IsCurrent { get; set; }

        public ulong GuildId { get; set; }

        public ICollection<Task> Tasks { get; set; }
    }
}