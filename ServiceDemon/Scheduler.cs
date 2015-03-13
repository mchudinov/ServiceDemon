using System;
using Quartz;
using log4net;
using Quartz.Impl;
using System.Configuration;

namespace ServiceDemon
{
    public class Scheduler
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ServiceDemon.Scheduler));
        static IScheduler _scheduler;

        public void Start()
        {
            ISchedulerFactory factory = new StdSchedulerFactory();
            _scheduler = factory.GetScheduler();
            _scheduler.Start();
            StartMyJob();
        }


        public void Shutdown()
        {
            if (null != _scheduler)
                _scheduler.Shutdown();
        }


        void StartMyJob()
        {
            var seconds = Int16.Parse(ConfigurationManager.AppSettings["MyJobSeconds"]);
            log.InfoFormat("Start MyJob. Execute once in {0} seconds", seconds);

            IJobDetail job = JobBuilder.Create<Jobs.MyJob>()
                .WithIdentity("MyJob", "group1")
                .UsingJobData("Param1", "Hello MyJob!")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("MyJobTrigger", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(seconds).RepeatForever())
                .Build();

            _scheduler.ScheduleJob(job, trigger);
        }

    }
}

