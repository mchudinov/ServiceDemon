using System;
using log4net;
using System.ServiceProcess;
using System.Diagnostics;

namespace ServiceDemon
{
    public class Service : ServiceBase
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ServiceDemon.Service));
        ServiceDemon.Scheduler _scheduler;

        public Service() 
        {
            ServiceName = "ServiceDemon";
            _scheduler = new ServiceDemon.Scheduler();
        }

        protected override void OnStart(string[] args)
        {
            log.Info("Service starting");
            _scheduler.Start();
        }

        protected override void OnStop()
        {
            log.Info("Service shutting down");
            _scheduler.Shutdown();
        }


        #if DEBUG
        // This method is for debugging of OnStart() method only.
        // Switch to Debug config, set a breakpoint here and a breakpoint in OnStart()
        // How to: Debug the OnStart Method http://msdn.microsoft.com/en-us/library/cktt23yw.aspx
        // How to: Debug Windows Service Applications http://msdn.microsoft.com/en-us/library/7a50syb3%28v=vs.110%29.aspx
        public static void Main(String[] args)
        {      
            (new ServiceDemon.Service()).OnStart(new string[1]);
            ServiceBase.Run( new ServiceDemon.Service() );
        }
        #endif
    }
}

