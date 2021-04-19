using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ScrumBot.Entities;

namespace ScrumBot.Models.Sprint
{
    public class SprintRepository : ISprintRepository
    {
        private readonly IScrumContext _context;

        public SprintRepository(IScrumContext context)
        {
            _context = context;
        }

        public async Task<int> Create(SprintDTO sprint)
        {
            //Set this sprint to the current sprint
            var sprintCreate = new Entities.Sprint
            {
                SprintName = sprint.SprintName,
                StartDate = sprint.StartDate,
                EndDate = sprint.EndDate,
                IsCurrent = true,
                GuildId = sprint.GuildId,
                Tasks = new List<Entities.Task>()
            };
            //Loop through prev sprints that are current and set them to false
            var prevCurrentSprints = _context.Sprints.Where(s => s.IsCurrent && s.GuildId == sprint.GuildId);
            foreach (var prevSprints in prevCurrentSprints) prevSprints.IsCurrent = false;

            _context.Sprints.Add(sprintCreate);
            await _context.SaveChangesAsync();
            return sprintCreate.Id;
        }

        public async Task<SprintDTO> Current(ulong guildId)
        {
            var sprint = await _context.Sprints.Include(s => s.Tasks)
                .FirstOrDefaultAsync(s => s.IsCurrent && s.GuildId == guildId);
            if (sprint == null) return null;
            var sprintReturn = new SprintDTO
            {
                Id = sprint.Id,
                SprintName = sprint.SprintName,
                IsCurrent = sprint.IsCurrent,
                StartDate = sprint.StartDate,
                EndDate = sprint.EndDate,
                GuildId = sprint.GuildId,
                Tasks = sprint.Tasks == null ? new List<int>() : sprint.Tasks.Select(us => us.Id).ToList()
            };
            return sprintReturn;
        }

        public async Task<IEnumerable<SprintDTO>> Read(ulong guildId)
        {
            return await _context.Sprints.Include(s => s.Tasks).Where(s => s.GuildId == guildId).Select(s =>
                new SprintDTO
                {
                    Id = s.Id,
                    SprintName = s.SprintName,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                    IsCurrent = s.IsCurrent,
                    Tasks = s.Tasks == null ? new List<int>() : s.Tasks.Select(us => us.Id).ToList()
                }).ToListAsync();
        }

        public async Task<SprintDTO> Read(ulong guildId, int sprintId)
        {
            var sprint = await _context.Sprints.Include(s => s.Tasks)
                .FirstOrDefaultAsync(s => s.Id == sprintId && s.GuildId == guildId);
            if (sprint == null) return null;
            return new SprintDTO
            {
                Id = sprint.Id,
                SprintName = sprint.SprintName,
                StartDate = sprint.StartDate,
                EndDate = sprint.EndDate,
                IsCurrent = sprint.IsCurrent,
                GuildId = sprint.GuildId,
                Tasks = sprint.Tasks == null ? new List<int>() : sprint.Tasks.Select(us => us.Id).ToList()
            };
        }

        public async System.Threading.Tasks.Task Update(SprintDTO sprint)
        {
            var sprintUpdate = await _context.Sprints.FirstOrDefaultAsync(s => s.Id == sprint.Id);
            if (sprintUpdate == null) return;

            sprintUpdate.SprintName = sprint.SprintName;
            sprintUpdate.StartDate = sprint.StartDate;
            sprintUpdate.EndDate = sprint.EndDate;
            sprintUpdate.IsCurrent = sprint.IsCurrent;
            sprintUpdate.GuildId = sprint.GuildId;
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task Delete(ulong guildId, int sprintId)
        {
            var sprint = await _context.Sprints.FirstOrDefaultAsync(s => s.Id == sprintId && s.GuildId == guildId);
            if (sprint == null) return;

            _context.Sprints.Remove(sprint);
            await _context.SaveChangesAsync();
        }
    }
}