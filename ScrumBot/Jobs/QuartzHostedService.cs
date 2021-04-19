using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;

namespace ScrumBot.Jobs
{
    //Followed tutorial at https://andrewlock.net/creating-a-quartz-net-hosted-service-with-asp-net-core/
    public class QuartzHostedService : IHostedService
    {
        private readonly IJobFactory _jobFactory;
        private readonly ReminderJob _reminderJob;
        private readonly ISchedulerFactory _schedulerFactory;

        public QuartzHostedService(
            ISchedulerFactory schedulerFactory, IJobFactory jobFactory, ReminderJob reminderJob)
        {
            _schedulerFactory = schedulerFactory;
            _jobFactory = jobFactory;
            _reminderJob = reminderJob;
        }

        public IScheduler Scheduler { get; set; }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
            Scheduler.JobFactory = _jobFactory;
            //Reminder Job
            var roleUpdateJob = JobBuilder
                .Create(_reminderJob.GetType())
                .WithIdentity(_reminderJob.GetType().FullName)
                .WithDescription(_reminderJob.GetType().Name)
                .Build();

            var roleUpdateTrigger = TriggerBuilder
                .Create()
                .WithIdentity($"{_reminderJob.GetType()}.trigger")
                .WithSchedule(
                    //This is where the timing for the schedule gets set, right now every minute
                    CronScheduleBuilder.CronSchedule("0 0/1 * 1/1 * ? *"))
                .Build();
            await Scheduler.ScheduleJob(roleUpdateJob, roleUpdateTrigger, cancellationToken);

            await Scheduler.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (Scheduler != null) await Scheduler?.Shutdown(cancellationToken);
        }
    }
}