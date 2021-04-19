using System;
using System.Threading.Tasks;

namespace ScrumBot.Models.Reminder
{
    public interface IReminderRepository
    {
        Task<(Status status, ReminderDTO reminder)> Get(DateTime startTime, ulong guildId, string summary);
        Task<(Status status, bool reminded)> IsReminded(DateTime startTime, ulong guildId, string summary);
        Task<(Status status, ReminderDTO reminder)> Create(ReminderDTO reminder);
    }
}