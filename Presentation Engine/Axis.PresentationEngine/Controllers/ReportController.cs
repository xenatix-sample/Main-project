using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web.Mvc;
using Axis.PresentationEngine.ReportServer;
using WebGrease.Css.Extensions;

namespace Axis.PresentationEngine.Controllers
{
    public class ReportController : Controller
    {
        public FileContentResult Index()
        {
            Dictionary<string, string> reportParameters = new Dictionary<string, string>();
            Request.QueryString.AllKeys.Where(x => !x.Equals("ReportName")).ForEach(x => reportParameters.Add(x, Request.QueryString[x]));

            byte[] byteResult = new byte[0];
            string extension, mimeType, encoding;
            Warning[] warnings = new Warning[0];
            string[] streamIds = new string[0];
            ServerInfoHeader serverInfoHeader;
            ExecutionInfo2 execInfo = new ExecutionInfo2();
            ReportExecutionServiceSoapClient rsServiceClient = new ReportExecutionServiceSoapClient();
            rsServiceClient.ClientCredentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Impersonation;
            ExecutionHeader execHeader = rsServiceClient.LoadReport2(null, "/Axis.ReportDB/" + Request.QueryString["ReportName"], null,
                out serverInfoHeader, out execInfo);

            rsServiceClient.SetExecutionParameters2(execHeader, null,
                reportParameters.Select(x => new ParameterValue {Name = x.Key, Value = x.Value}).ToArray(), "en-us",
                out execInfo);
            string sessionId = execInfo.ExecutionID;
            try
            {
                serverInfoHeader = rsServiceClient.Render2(execHeader, null, "PDF", @"<DeviceInfo><Toolbar>True</Toolbar></DeviceInfo>", PageCountMode.Actual, out byteResult, out extension, out mimeType, out encoding, out warnings, out streamIds);
            }
            catch (Exception)
            {
                throw;
            }
            return new FileContentResult(byteResult, "application/pdf");
        }
    }
}