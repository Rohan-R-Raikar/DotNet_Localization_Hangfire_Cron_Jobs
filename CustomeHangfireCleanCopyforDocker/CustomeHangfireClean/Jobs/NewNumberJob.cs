using CustomeHangfireClean.Data;

namespace CustomeHangfireClean.Jobs
{
    public class NewNumberJob
    {
        private readonly AppDbContext _db;

        public NewNumberJob(AppDbContext db)
        {
            _db = db;
        }

        public void Execute()
        {
            var counter = _db.JobCounters.FirstOrDefault();
            if (counter == null || counter.Number == 0)
            {
                counter = new Models.JobCounter { NewNumber = 1 };
                _db.JobCounters.Add(counter);
            }
            else
            {
                counter.NewNumber = counter.Square - counter.Number;
                _db.JobCounters.Update(counter);
            }

            _db.SaveChanges();
        }
    }
}
