using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ScrumBot.Entities;

namespace ScrumBot.Models.Reminder
{
    public class ReminderRepository : IReminderRepository
    {
        private readonly IScrumContext _context;

        public ReminderRepository(IScrumContext context)
        {
            _context = context;
        }

        public async Task<(Status status, ReminderDTO reminder)> Get(DateTime startTime, ulong guildId, string summary)
        {
            var reminder = await _context.Reminders.FirstOrDefaultAsync(r =>
                r.StartTime.Equals(startTime) && r.Summary.ToLower().Equals(summary.ToLower()) && r.GuildId == guildId);

            if (reminder == null) return (Status.BadRequest, null);

            return (Status.Found, new ReminderDTO
            {
                StartTime = reminder.StartTime,
                GuildId = reminder.GuildId,
                Summary = reminder.Summary,
                Reminded = reminder.Reminded
            });
        }

        public async Task<(Status status, bool reminded)> IsReminded(DateTime startTime, ulong guildId, string summary)
        {
            var existingReminder = await Get(startTime, guildId, summary);
            if (existingReminder.status != Status.Found) return (Status.BadRequest, false);

            return (Status.Found, existingReminder.reminder.Reminded);
        }

        public async Task<(Status status, ReminderDTO reminder)> Create(ReminderDTO reminder)
        {
            var existingReminder = await Get(reminder.StartTime, reminder.GuildId, reminder.Summary);
            if (existingReminder.status == Status.Found) return (Status.Conflict, existingReminder.reminder);

            var reminderCreate = new Entities.Reminder
            {
                StartTime = reminder.StartTime,
                GuildId = reminder.GuildId,
                Summary = reminder.Summary,
                Reminded = reminder.Reminded
            };
            var createdReminder = await _context.Reminders.AddAsync(reminderCreate);
            await _context.SaveChangesAsync();

            if (createdReminder.Entity == null) return (Status.Error, null);

            return (Status.Created, new ReminderDTO
            {
                StartTime = createdReminder.Entity.StartTime,
                GuildId = createdReminder.Entity.GuildId,
                Summary = createdReminder.Entity.Summary,
                Reminded = createdReminder.Entity.Reminded
            });
        }
    }
}