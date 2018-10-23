using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ServiceMonitorStatus
{
    internal class ServiceUp : IDisposable
    {
        private  ServiceController _service { get; set; }
        private Task _internalTask { get; set; }
        internal bool RunningMonitor = false;
        internal string ServiceName { get; set; }

        internal ServiceUp(string serviceName)
        {
            ServiceName = serviceName;
            _service =  new ServiceController(serviceName);
        }

      
        internal void StartMonitorStopped()
        {
            if(RunningMonitor)
            {
                RunningMonitor = false;
                _internalTask?.Dispose();
            }
                    
            _internalTask = Task.Factory.StartNew(async () => {
                RunningMonitor = true;

                if(_service.Status != ServiceControllerStatus.Running)
                {
                    _service.Start();
                }

                while (true)
                {
                    _service.WaitForStatus(ServiceControllerStatus.Stopped);
                    await Task.Delay(3000);
                    _service.Start();
                }         
            });
        }

        public void Dispose()
        {   
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _internalTask?.Dispose();
                _internalTask = null;
                _service = null;
                RunningMonitor = false;
                ServiceName = string.Empty;
            }
        }
    }
}
