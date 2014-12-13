using System;
using log4net;
using System.ServiceProcess;
using Quartz;
using Quartz.Impl;
using System.Configuration;
using System.Diagnostics;

namespace ServiceDemon
{
    public class Service : ServiceBase
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ServiceDemon.Service));
        static IScheduler Scheduler { get; set; }

        public Service() {}

        protected override void OnStart(string[] args)
        {
            log.Info("Service starting");
            StartScheduler();
            StartMyJob();
        }

        protected override void OnStop()
        {
            log.Info("Service shutting down");
            Scheduler.Shutdown();
        }

        void StartScheduler()
        {
            ISchedulerFactory schedFact = new StdSchedulerFactory();
            Scheduler = schedFact.GetScheduler();
            Scheduler.Start();
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

            Scheduler.ScheduleJob(job, trigger);
        }

        #if DEBUG
        // This method is for debugging of OnStart() method only.
        // Switch to Debug config, set a breakpoint here and a breakpoint in OnStart()
        // How to: Debug the OnStart Method http://msdn.microsoft.com/en-us/library/cktt23yw.aspx
        // How to: Debug Windows Service Applications http://msdn.microsoft.com/en-us/library/7a50syb3%28v=vs.110%29.aspx
        [Conditional("DEBUG")]
        public static void Main(String[] args)
        {      
            (new ServiceDemon.Service()).OnStart(new string[1]);
            ServiceBase.Run( new ServiceDemon.Service() );
        }
        #endif
    }
}

