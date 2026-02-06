using CustomeHangfireClean.Data;

namespace CustomeHangfireClean.Jobs
{
    public class SquareJob
    {
        private readonly AppDbContext _db;

        public SquareJob(AppDbContext db)
        {
            _db = db;
        }

        public void Execute()
        {
            var counter = _db.JobCounters.FirstOrDefault();
            if (counter == null || counter.Number == 0)
            {
                counter = new Models.JobCounter { Square = 1 };
                _db.JobCounters.Add(counter);
            }
            else
            {
                counter.Square = counter.Number * 3;
                _db.JobCounters.Update(counter);
            }

            _db.SaveChanges();
        }
    }
}
