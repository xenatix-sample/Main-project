using log4net;
using System;
using System.Configuration;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Axis.Security;
using Axis.Model.Account;
using Microsoft.ApplicationInsights.DataContracts;

namespace Axis.Logging
{
    public class Logger : ILogger
    {
        public ILog log;
        private bool _enableLogs;
        private bool _enableAppInsights;
        private TelemetryClient telemetry;
        private string _userName;

        public Logger()
        {
            _enableLogs = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("EnableLogging"));
            _enableAppInsights = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("EnableAppInsights"));
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            _userName = AuthContext.Auth.User.UserName;
            InitAppInsight();
        }

        public Logger(bool enableLogging)
        {
            _enableLogs = enableLogging;
            _enableAppInsights = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("EnableAppInsights"));
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            _userName = AuthContext.Auth.User.UserName;
            InitAppInsight();
           
        }

        public void Info(string message)
        {
            if (_enableLogs)
            {
                log.Info(message);
            }
            if (_enableAppInsights)
                telemetry.TrackTrace(message);

        }
        public void Warn(string message)
        {
            if (_enableLogs)
            {
                log.Warn(message);
            }

            if (_enableAppInsights)
                telemetry.TrackTrace(message, Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Warning);
        }
        public void Debug(string message)
        {
            if (_enableLogs)
            {
                log.Debug(message);
            }

            if (_enableAppInsights)
                telemetry.TrackTrace(message, Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Information);
        }
        public void Error(string message)
        {
            if (_enableLogs)
            {
                log.Error(message);
            }

            if (_enableAppInsights)
                telemetry.TrackTrace(message, Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Error);
        }
        public void Error(Exception ex)
        {
            if (_enableLogs)
            {
                log.Error(ex.Message, ex);
            }

            if (_enableAppInsights)
                telemetry.TrackException(ex);
        }

        public void Error(string message, Exception ex)
        {
            if (_enableLogs)
            {
                log.Error(message, ex);
             }

            if (_enableAppInsights)
            {
                var exceptionTelemetry = new ExceptionTelemetry(ex);
                exceptionTelemetry.Properties.Add("message", message);
                telemetry.TrackException(exceptionTelemetry);
            }
        }

        public void Fatal(string message)
        {
            if (_enableLogs)
            {
                log.Fatal(message);
            }
        }
        public void Fatal(Exception ex)
        {
            if (_enableLogs)
            {
                log.Fatal(ex.Message, ex);
            }
        }

        private void InitAppInsight()
        {
            if (_enableAppInsights)
            {
                telemetry = new TelemetryClient();
                telemetry.InstrumentationKey = ConfigurationManager.AppSettings.Get("AppInsightsKey");
                telemetry.Context.User.Id = _userName;
            }
        }
    }
}
