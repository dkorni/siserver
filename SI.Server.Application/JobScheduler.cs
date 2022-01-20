using System;
using System.Collections.Generic;
using SI.Server.Domain.Interfaces;

namespace SI.Server.Application
{
    public class JobScheduler
    {
        private Dictionary<Guid, IJob> _jobs;

        public Guid RunNewJob(IJob job)
        {
            var jobId = Guid.NewGuid();
            job.Run();
            return jobId;
        }

        public bool TryStopJob(Guid jobId)
        {
            var isExisting = _jobs.TryGetValue(jobId, out var job);
            if(job != null)
                job.Stop();
            return isExisting;
        }
    }
}