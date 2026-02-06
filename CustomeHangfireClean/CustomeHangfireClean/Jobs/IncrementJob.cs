using CustomeHangfireClean.Data;
using Hangfire.Common;
using Hangfire.States;
using Hangfire.Storage;

namespace CustomeHangfireClean.Jobs
{
    public class IncrementJob
    {
        private readonly AppDbContext _db;

        public IncrementJob(AppDbContext db)
        {
            _db = db;
        }

        public void Execute()
        {
            var counter = _db.JobCounters.FirstOrDefault();
            if (counter == null)
            {
                counter = new Models.JobCounter { Number = 1 };
                _db.JobCounters.Add(counter);
            }
            else
            {
                counter.Number += 5;
                _db.JobCounters.Update(counter);
            }

            _db.SaveChanges();
        }
    }
}
