using Hangfire.Common;
using Hangfire.States;
using Hangfire.Storage;

namespace CustomeHangfireClean.Jobs
{
    public class CustomeExpirationTimeAttribute : JobFilterAttribute, IApplyStateFilter
    {
        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            context.JobExpirationTimeout = GetExpiration(context.Job.Type.Name);
        }

        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            context.JobExpirationTimeout = GetExpiration(context.Job.Type.Name);
        }

        private TimeSpan GetExpiration(string jobTypeName)
        {
            return jobTypeName switch
            {
                "IncrementJob" => TimeSpan.FromDays(365),
                "SquareJob" => TimeSpan.FromDays(183),
                "NewNumberJob" => TimeSpan.FromDays(90),
                _ => TimeSpan.FromDays(15)
            };
        }
    }
}
