using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ScrumBot.Entities;
using ScrumBot.Models.Task;
using Xunit;

namespace ScrumBot.Tests.Repositories
{
    public class TaskRepositoryTests
    {
        private readonly TaskRepository _taskRepository;

        public TaskRepositoryTests()
        {
            //Connection
            var connection = new SqliteConnection("datasource=:memory:");
            connection.Open();

            //Context
            var builder = new DbContextOptionsBuilder<ScrumContext>().UseSqlite(connection);
            var context = new ScrumTestContext(builder.Options);
            context.Database.EnsureCreated();

            _taskRepository = new TaskRepository(context);
        }

        [Fact]
        public async void Test_Correct_Amount_Of_Sprints()
        {
            var actual = await _taskRepository.Read();
            Assert.Equal(4, actual.Count());
        }
    }
}