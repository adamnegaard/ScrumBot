using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ScrumBot.Entities;
using ScrumBot.Models.Sprint;
using Xunit;

namespace ScrumBot.Tests.Repositories
{
    public class SprintRepositoryTests
    {
        private readonly SprintRepository _sprintRepository;

        public SprintRepositoryTests()
        {
            //Connection
            var connection = new SqliteConnection("datasource=:memory:");
            connection.Open();

            //Context
            var builder = new DbContextOptionsBuilder<ScrumContext>().UseSqlite(connection);
            var context = new ScrumTestContext(builder.Options);
            context.Database.EnsureCreated();

            _sprintRepository = new SprintRepository(context);
        }

        [Fact]
        public async void Test_Correct_Amount_Of_Sprints()
        {
            var actual = await _sprintRepository.Read(1);
            Assert.Equal(2, actual.Count());
        }
    }
}