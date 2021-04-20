using System;
using Microsoft.EntityFrameworkCore;
using ScrumBot.Entities;

namespace ScrumBot.Tests.Repositories
{
    public class ScrumTestContext : ScrumContext
    {
        public ScrumTestContext(DbContextOptions<ScrumContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Sprint>(entity =>
            {
                entity.HasData(new Sprint
                {
                    Id = 1,
                    SprintName = "Sprint 1",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    IsCurrent = false,
                    GuildId = 1
                }, new Sprint
                {
                    Id = 2,
                    SprintName = "Sprint 2",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    IsCurrent = true,
                    GuildId = 1
                });
            });

            modelBuilder.Entity<Task>(entity =>
                entity.HasData(new Task
                {
                    Id = 1,
                    TaskName = "a",
                    SprintId = 1,
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now,
                    DiscordRoleId = 1
                }, new Task
                {
                    Id = 2,
                    TaskName = "b",
                    SprintId = 1,
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now,
                    DiscordRoleId = 2
                }, new Task
                {
                    Id = 3,
                    TaskName = "c",
                    SprintId = 2,
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now,
                    DiscordRoleId = 3
                }, new Task
                {
                    Id = 4,
                    TaskName = "d",
                    SprintId = 2,
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now,
                    DiscordRoleId = 4
                }));

            modelBuilder.Entity<Reminder>(entity =>
                entity.HasData(new Reminder
                {
                    StartTime = DateTime.Now,
                    GuildId = 1,
                    Summary = "a",
                    Reminded = true
                }, new Reminder
                {
                    StartTime = DateTime.Now,
                    GuildId = 1,
                    Summary = "b",
                    Reminded = true
                }, new Reminder
                {
                    StartTime = DateTime.Now,
                    GuildId = 1,
                    Summary = "c",
                    Reminded = true
                }));
        }
    }
}