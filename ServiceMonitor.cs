using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ServiceMonitorStatus
{
    public partial class ServiceMonitor : ServiceBase
    {
        private  List<ServiceUp> _servicesControl { get; set; }
        private  List<ServiceController> _serviceSystem { get; set; }

        public ServiceMonitor()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            StartedService();
        }

        internal void StartedService()
        {
            _serviceSystem = ServiceController.GetServices().ToList();
            _servicesControl = new List<ServiceUp>();
            string[] serviceNames = System.Configuration.ConfigurationManager.AppSettings["ServiceSupport"].ToString()?.Split(',');
            Task.Factory.StartNew(() => {
                serviceNames.ToList().ForEach(it => {
                    AddService(it);
                });
            });
        }

        protected override void OnStop()
        {
            _servicesControl?.ForEach(it => { it.Dispose(); });
            _servicesControl = null;
            _serviceSystem = null;
        }

        private void AddService(string serviceName)
        {
            bool existServiceName = _servicesControl?.Any(it => it.ServiceName == serviceName) ?? false;
            bool existServiceSysem = _serviceSystem?.Any(it => it.ServiceName == serviceName) ?? false;
            if (!existServiceName && existServiceSysem)
            {
                ServiceUp serviceUp = new ServiceUp(serviceName);
                serviceUp.StartMonitorStopped();
                _servicesControl.Add(serviceUp);
            }
        }
    }
}
