using Hangfire;
using Hangfire.Storage;
using System;
using System.Linq;

namespace CustomeHangfireClean.Jobs
{
    public class DailyEMIJob
    {
        private static readonly object _initLock = new object();
        private static bool _backlogProcessed = false;

        public void Execute()
        {
            try
            {
                if (!_backlogProcessed)
                {
                    lock (_initLock)
                    {
                        if (!_backlogProcessed)
                        {
                            using (var connection = JobStorage.Current.GetConnection())
                            {
                                using (connection.AcquireDistributedLock("DailyEMIJob:Drain", TimeSpan.FromMinutes(5)))
                                {
                                    var monitoringApi = JobStorage.Current.GetMonitoringApi();
                                    var scheduledJobs = monitoringApi.ScheduledJobs(0, 10);
                                    var backlogIds = scheduledJobs
                                        .Where(kv => kv.Value.Job.Type == typeof(DailyEMIJob) && kv.Value.Job.Method.Name == "Execute")
                                        .Select(kv => kv.Key)
                                        .ToList();

                                    foreach (var id in backlogIds)
                                        BackgroundJob.Delete(id);

                                    foreach (var id in backlogIds)
                                        Run(false);
                                }
                            }

                            _backlogProcessed = true;
                        }
                    }
                }
                Run(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Daily EMI job failed: {ex.Message}");
            }
        }

        private void Run(bool allowSchedule)
        {
            Console.WriteLine("Daily EMI job executed successfully!");

            if (allowSchedule)
            {
                if (!IsJobAlreadyScheduled())
                {
                    BackgroundJob.Schedule<DailyEMIJob>(job => job.Execute(), TimeSpan.FromSeconds(60));
                    Console.WriteLine("Next Daily EMI job scheduled successfully.");
                }
            }
        }

        private bool IsJobAlreadyScheduled()
        {
            var monitoringApi = JobStorage.Current.GetMonitoringApi();
            var scheduledJobs = monitoringApi.ScheduledJobs(0, 10);
            return scheduledJobs.Any(kv => kv.Value.Job.Type == typeof(DailyEMIJob) && kv.Value.Job.Method.Name == "Execute");
        }
    }
}
