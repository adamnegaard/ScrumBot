using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ScrumBot.Entities;
using ScrumBot.Models.Reminder;

namespace ScrumBot.Tests.Repositories
{
    public class ReminderRepositoryTests
    {
        private readonly ReminderRepository _reminderRepository;

        public ReminderRepositoryTests()
        {
            //Connection
            var connection = new SqliteConnection("datasource=:memory:");
            connection.Open();

            //Context
            var builder = new DbContextOptionsBuilder<ScrumContext>().UseSqlite(connection);
            var context = new ScrumTestContext(builder.Options);
            context.Database.EnsureCreated();

            _reminderRepository = new ReminderRepository(context);
        }
    }
}