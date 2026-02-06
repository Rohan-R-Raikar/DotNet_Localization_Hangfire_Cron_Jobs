using Hangfire;

namespace CustomeHangfireClean.Jobs
{
    public class Test2Job
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
                                using (connection.AcquireDistributedLock("Test2Job:Drain", TimeSpan.FromMinutes(5)))
                                {
                                    var monitoringApi = JobStorage.Current.GetMonitoringApi();
                                    var scheduledJobs = monitoringApi.ScheduledJobs(0, 10);
                                    var backlogIds = scheduledJobs
                                        .Where(kv => kv.Value.Job.Type == typeof(Test2Job) && kv.Value.Job.Method.Name == "Execute")
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
                Console.WriteLine($"Test2Job failed: {ex.Message}");
            }
        }

        private void Run(bool allowSchedule)
        {
            Console.WriteLine("Test2Job executed successfully!");

            if (allowSchedule)
            {
                if (!IsJobAlreadyScheduled())
                {
                    BackgroundJob.Schedule<Test2Job>(job => job.Execute(), TimeSpan.FromSeconds(60));
                    Console.WriteLine("Next Test2Job scheduled successfully.");
                }
            }
        }

        private bool IsJobAlreadyScheduled()
        {
            var monitoringApi = JobStorage.Current.GetMonitoringApi();
            var scheduledJobs = monitoringApi.ScheduledJobs(0, 10);
            return scheduledJobs.Any(kv => kv.Value.Job.Type == typeof(Test2Job) && kv.Value.Job.Method.Name == "Execute");
        }
        //public void Execute()
        //{
        //    try
        //    {
        //        Console.WriteLine("Test2Job executed successfully!");

        //        BackgroundJob.Schedule<Test2Job>(
        //            job => job.Execute(),
        //            TimeSpan.FromSeconds(10)
        //        );

        //        Console.WriteLine("Next Test2Job scheduled successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Daily EMI job failed: {ex.Message}");
        //    }
        //}

    }
}
