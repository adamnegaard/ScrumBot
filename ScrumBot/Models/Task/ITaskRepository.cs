using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace ScrumBot.Models.Task
{
    public interface ITaskRepository
    {
        Task<int> Create(TaskDTO task);
        Task<IEnumerable<TaskDTO>> Read();
        Task<IEnumerable<TaskDTO>> Read(IEnumerable<int> taskIdList);
        Task<TaskDTO> Read(int taskId);
        Task<TaskDTO> Read(DiscordRole role);
        System.Threading.Tasks.Task Update(TaskDTO task);
        System.Threading.Tasks.Task Delete(int taskId);
    }
}