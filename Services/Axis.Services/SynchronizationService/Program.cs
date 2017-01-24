using Axis.Constant;
using Axis.DataProvider.SynchronizationService;
using Axis.Logging;
using SynchronizationService.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;

namespace SynchronizationService
{
    static class Program
    {
        //DebgugConfigID helps return the Config for ActiveDirectory from Synch.Config Table
        private const int DebugConfigID = 1;
        //To Enable the Event Viewer Logging
        private static bool debugMode = ConfigurationManager.AppSettings["Debug"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["Debug"]);
        //To Enable the Text File Logging mainly in Production Environment - Think about clean up procedures or truncating the file
        private static bool debugLogFileMode = ConfigurationManager.AppSettings["DebugLogFile"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["DebugLogFile"]);
        private static ILogger _logger;

        static void Main()
        {
            try
            {
                //Begin - Section 1: Enable Logging
                EnableLogging();
                //End - Section 1

                //Begin Section 2: - Get SSDP object
                var syncServDp = new SynchronizationServiceDataProvider(_logger);
                var syncServices = syncServDp.GetServiceConfigurations();
                //End - Section 2

                //Begin Section 3: - Start The Service
                if (Debugger.IsAttached)
                {
                    var debugConfig = syncServDp.GetServiceConfiguration(DebugConfigID);
                    //Launch the Service
                    LDAPSyncService.Sync(debugConfig.DataItems[0].ConfigID);
                }
                else
                {
                    var servicesToRun = new List<ServiceBase>();

                    foreach (var serviceConfigurationModel in syncServices.DataItems)
                    {
                        switch (serviceConfigurationModel.ConfigTypeID)
                        {
                            //This determines all the service configuration for the LDAP that needs to be run
                            case ((int)ConfigType.LDAP):
                                {
                                    if (debugMode) EventLog.WriteEntry("SynchronizationService", "Adding service for ConfigXML " + serviceConfigurationModel.ConfigXML, EventLogEntryType.Information);

                                    if (debugLogFileMode) using (StreamWriter sw = File.AppendText("Log.txt"))
                                    {
                                        sw.WriteLine("Adding service for ConfigXML " + serviceConfigurationModel.ConfigXML);
                                    }
                                     
                                    servicesToRun.Add(new LDAPSynchronizationService(serviceConfigurationModel.ConfigID));
                                    break;
                                }
                        }
                    };

                    ServiceBase.Run(servicesToRun.ToArray());
                }
                //End Section 3
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SynchronizationService", "Error launching services: " + e.Message, EventLogEntryType.Error);
            }
        }
        static void EnableLogging()
        {
            if (debugMode) EventLog.WriteEntry("SynchronizationService",
                "Starting service, attempting to connect to Axis db using connection string " + ConfigurationManager.AppSettings["XenatixDBConnection"],
                EventLogEntryType.Information);

            if (debugLogFileMode)
            {
                if (!File.Exists("Log.txt"))
                {
                    using (var sw = File.Create("Log.txt"))
                    {
                    }
                }
                using (StreamWriter sw = File.AppendText("Log.txt"))
                {
                    sw.WriteLine("Starting service, attempting to connect to Axis db using connection string " +
                        ConfigurationManager.ConnectionStrings["XenatixDBConnection"].ToString());
                }
            }
        }
    }
}
