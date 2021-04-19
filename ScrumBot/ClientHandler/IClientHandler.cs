using System.Threading.Tasks;
using DSharpPlus;

namespace ScrumBot.ClientHandler
{
    public interface IClientHandler
    {
        Task SendMeetingReminder(DiscordClient client);
    }
}