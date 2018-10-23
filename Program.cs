using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ServiceMonitorStatus
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if (!DEBUG)
           ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ServiceMonitor()
            };
            ServiceBase.Run(ServicesToRun);

#else
            ServiceMonitor myServ = new ServiceMonitor();
            myServ.StartedService();
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
#endif
           
        }
    }
}
