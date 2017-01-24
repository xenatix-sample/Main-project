using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Timers;
using System.Xml;
using Axis.DataProvider.SynchronizationService;
using SynchronizationService.Services;
using Axis.Logging;

namespace SynchronizationService
{
    public partial class LDAPSynchronizationService : ServiceBase
    {
        protected ILogger _logger;
        readonly Timer _timerClock = new Timer();
        readonly int _configID;

        public LDAPSynchronizationService(int configID)
        {
            _configID = configID;
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var syncServ = new SynchronizationServiceDataProvider(_logger);
            var config = syncServ.GetServiceConfiguration(_configID).DataItems[0];

            using (XmlReader reader = XmlReader.Create(new StringReader(config.ConfigXML)))
            {
                // get the services that need to be started, and their intervals, from the db
                reader.ReadToFollowing("Timer");
                reader.ReadToDescendant("Interval");
                reader.MoveToFirstAttribute();
                var interval = Convert.ToDouble(reader.Value);

                _timerClock.Elapsed += delegate { LDAPSyncService.Sync(_configID); };
                _timerClock.Interval = interval * 60 * 1000; // interval (in minutes) to execute process
                _timerClock.Enabled = true;
            }
        }

        protected override void OnStop()
        {
            _timerClock.Enabled = false;
        }
    }
}
