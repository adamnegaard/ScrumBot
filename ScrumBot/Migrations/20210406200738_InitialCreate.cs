using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ScrumBot.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Reminders",
                table => new
                {
                    StartTime = table.Column<DateTime>("timestamp without time zone", nullable: false),
                    GuildId = table.Column<decimal>("numeric(20,0)", nullable: false),
                    Summary = table.Column<string>("text", nullable: true),
                    Reminded = table.Column<bool>("boolean", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Reminders", x => new {x.StartTime, x.GuildId}); });

            migrationBuilder.CreateTable(
                "Sprints",
                table => new
                {
                    Id = table.Column<int>("integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    SprintName = table.Column<string>("text", nullable: false),
                    StartDate = table.Column<DateTime>("timestamp without time zone", nullable: false),
                    EndDate = table.Column<DateTime>("timestamp without time zone", nullable: false),
                    IsCurrent = table.Column<bool>("boolean", nullable: false),
                    GuildId = table.Column<decimal>("numeric(20,0)", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Sprints", x => x.Id); });

            migrationBuilder.CreateTable(
                "Tasks",
                table => new
                {
                    Id = table.Column<int>("integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    TaskName = table.Column<string>("text", nullable: false),
                    SprintId = table.Column<int>("integer", nullable: false),
                    StartTime = table.Column<DateTime>("timestamp without time zone", nullable: false),
                    EndTime = table.Column<DateTime>("timestamp without time zone", nullable: false),
                    DiscordRoleId = table.Column<decimal>("numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        "FK_Tasks_Sprints_SprintId",
                        x => x.SprintId,
                        "Sprints",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_Tasks_SprintId",
                "Tasks",
                "SprintId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Reminders");

            migrationBuilder.DropTable(
                "Tasks");

            migrationBuilder.DropTable(
                "Sprints");
        }
    }
}