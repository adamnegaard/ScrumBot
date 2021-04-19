using Microsoft.EntityFrameworkCore.Migrations;

namespace ScrumBot.Migrations
{
    public partial class ReminderAddSummaryKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                "PK_Reminders",
                "Reminders");

            migrationBuilder.AlterColumn<string>(
                "Summary",
                "Reminders",
                "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                "PK_Reminders",
                "Reminders",
                new[] {"StartTime", "GuildId", "Summary"});
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                "PK_Reminders",
                "Reminders");

            migrationBuilder.AlterColumn<string>(
                "Summary",
                "Reminders",
                "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                "PK_Reminders",
                "Reminders",
                new[] {"StartTime", "GuildId"});
        }
    }
}