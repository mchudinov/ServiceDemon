using System;
using System.ServiceProcess;

namespace ServiceDemon
{
    class Program
    {
        #if (DEBUG != true)
        public static void Main(string[] args)
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] { new ServiceDemon.Service() };
            ServiceBase.Run(ServicesToRun);
        }
        #endif
    }
}
