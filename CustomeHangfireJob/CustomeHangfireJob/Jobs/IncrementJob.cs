using CustomeHangfireJob.Data;
using CustomeHangfireJob.Models;

namespace CustomeHangfireJob.Jobs
{
    public class IncrementJob
    {
        private readonly AppDbContext _context;

        public IncrementJob(AppDbContext context)
        {
            _context = context;
        }

        public void Execute()
        {
            // Get the first row from table
            var counter = _context.Counters.FirstOrDefault();
            if (counter == null)
            {
                counter = new Counter { Number = 1 };
                _context.Counters.Add(counter);
            }
            else
            {
                counter.Number += 1;
            }

            _context.SaveChanges();
            Console.WriteLine($"Number incremented: {counter.Number}");
        }
    }
}
