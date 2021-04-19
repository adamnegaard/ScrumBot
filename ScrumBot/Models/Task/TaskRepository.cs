using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;
using ScrumBot.Entities;

namespace ScrumBot.Models.Task
{
    public class TaskRepository : ITaskRepository
    {
        private readonly IScrumContext _context;

        public TaskRepository(IScrumContext context)
        {
            _context = context;
        }

        public async Task<int> Create(TaskDTO task)
        {
            var TaskCreate = new Entities.Task
            {
                TaskName = task.TaskName,
                SprintId = task.SprintId,
                StartTime = task.StartTime,
                EndTime = task.EndTime,
                DiscordRoleId = task.DiscordRoleId
            };
            await _context.Tasks.AddAsync(TaskCreate);
            await _context.SaveChangesAsync();
            return TaskCreate.Id;
        }

        public async Task<IEnumerable<TaskDTO>> Read()
        {
            return await _context.Tasks.Select(t => new TaskDTO
            {
                Id = t.Id,
                TaskName = t.TaskName,
                SprintId = t.SprintId,
                StartTime = t.StartTime,
                EndTime = t.EndTime,
                DiscordRoleId = t.DiscordRoleId
            }).ToListAsync();
        }

        public async Task<IEnumerable<TaskDTO>> Read(IEnumerable<int> taskIdList)
        {
            return await _context.Tasks.Where(t => taskIdList.Contains(t.Id)).Select(t => new TaskDTO
            {
                Id = t.Id,
                TaskName = t.TaskName,
                SprintId = t.SprintId,
                StartTime = t.StartTime,
                EndTime = t.EndTime,
                DiscordRoleId = t.DiscordRoleId
            }).ToListAsync();
        }

        public async Task<TaskDTO> Read(int taskId)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
            if (task == null) return null;
            return new TaskDTO
            {
                Id = task.Id,
                TaskName = task.TaskName,
                SprintId = task.SprintId,
                StartTime = task.StartTime,
                EndTime = task.EndTime,
                DiscordRoleId = task.DiscordRoleId
            };
        }

        public async Task<TaskDTO> Read(DiscordRole role)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.DiscordRoleId == role.Id);
            if (task == null) return null;
            return new TaskDTO
            {
                Id = task.Id,
                TaskName = task.TaskName,
                SprintId = task.SprintId,
                StartTime = task.StartTime,
                EndTime = task.EndTime,
                DiscordRoleId = task.DiscordRoleId
            };
        }

        public async System.Threading.Tasks.Task Update(TaskDTO task)
        {
            var taskUpdate = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == task.Id);
            if (task == null) return;

            taskUpdate.TaskName = task.TaskName;
            taskUpdate.SprintId = task.SprintId;
            taskUpdate.StartTime = task.StartTime;
            taskUpdate.EndTime = task.EndTime;
            taskUpdate.DiscordRoleId = task.DiscordRoleId;
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task Delete(int taskId)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
            if (task == null) return;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }
}