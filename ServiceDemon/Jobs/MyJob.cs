using Quartz;
using log4net;

namespace ServiceDemon.Jobs
{
    public class MyJob : IJob
    {
        static readonly ILog log = LogManager.GetLogger(typeof(Program));

        public void Execute(IJobExecutionContext context)
        {
            log.Info("My job executed");
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            log.Info(dataMap["Param1"]);
        }
    }
}

