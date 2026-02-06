using Hangfire;
using Hangfire.Storage.Monitoring;
using HangfireCronDotNet9.Jobs;
using Microsoft.AspNetCore.Mvc;

namespace HangfireCronDotNet9.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        [HttpPost("CreateBackgroundJob")]
        public ActionResult CreateBackgroundJob()
        {
            //BackgroundJob.Enqueue(() => Console.WriteLine("Background Job Triggered"));
            BackgroundJob.Enqueue<TestJob> (x => x.WriteLog("Background Job Triggered"));
            return Ok();
        }

        [HttpPost("ScheduledJob")]
        public ActionResult ScheduledJob()
        {
            var scheduleDateTime = DateTime.UtcNow.AddSeconds(5);
            var dateTimeOffset = new DateTimeOffset(scheduleDateTime);
            //BackgroundJob.Schedule(() => Console.WriteLine("Scheduled Job Triggered"), dateTimeOffset);
            BackgroundJob.Schedule<TestJob>(x => x.WriteLog("Scheduled Job Triggered"), dateTimeOffset);
            return Ok();
        }

        [HttpPost("CreateContinuousJob")]
        public ActionResult CreateContinuousJob()
        {
            var scheduleDateTime = DateTime.UtcNow.AddSeconds(5);
            var dateTimeOffset = new DateTimeOffset(scheduleDateTime);
            var jobId = BackgroundJob.Schedule<TestJob>(x => x.WriteLog("Continues Job 1 Triggered"), dateTimeOffset);
            //var jobId = BackgroundJob.Schedule(() => Console.WriteLine("Continues Job 1 Triggered"), dateTimeOffset);

            var job2Id = BackgroundJob.ContinueJobWith<TestJob>(jobId, x => x.WriteLog("Continues Job 2 Triggered"));
            var job3Id = BackgroundJob.ContinueJobWith<TestJob>(job2Id, x => x.WriteLog("Continues Job 3 Triggered"));
            var job4Id = BackgroundJob.ContinueJobWith<TestJob>(job3Id, x => x.WriteLog("Continues Job 4 Triggered"));

            return Ok();
        }

        [HttpPost("CreateRecurringJob")]
        public ActionResult CreateRecurringJob()
        {
            //RecurringJob.AddOrUpdate("RecurringJob", () => Console.WriteLine("Recurring Job Job 1 Triggered"),"* * * * *");
            RecurringJob.AddOrUpdate<TestJob>("RecurringJob", x => x.WriteLog("Recurring Job Job 1 Triggered"),"* * * * *");
            return Ok();
        }
    }
}
