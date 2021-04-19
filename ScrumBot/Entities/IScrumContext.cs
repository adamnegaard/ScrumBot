using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ScrumBot.Entities
{
    public interface IScrumContext
    {
        DbSet<Sprint> Sprints { get; set; }
        DbSet<Task> Tasks { get; set; }
        DbSet<Reminder> Reminders { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}