using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScrumBot.Models.Sprint
{
    public interface ISprintRepository
    {
        Task<int> Create(SprintDTO sprint);
        Task<SprintDTO> Current(ulong guildId);
        Task<IEnumerable<SprintDTO>> Read(ulong guildId);
        Task<SprintDTO> Read(ulong guildId, int sprintId);
        System.Threading.Tasks.Task Update(SprintDTO sprint);
        System.Threading.Tasks.Task Delete(ulong guildId, int sprintId);
    }
}