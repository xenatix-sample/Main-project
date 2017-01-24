using Axis.Model.ReportingServices;
using Axis.Plugins.ReportingServices.Respository;
using Axis.PresentationEngine.Helpers.Controllers;
using System;
using System.IO;
using System.Text;
using System.Web.Mvc;

namespace Axis.Plugins.ReportingServices.Controllers
{
    public class ReportingServicesController : BaseController
    {
        #region Class Variables

        private readonly IReportingServicesRepository _reportingServicesRepository;

        #endregion

        #region Constructors

        public ReportingServicesController(IReportingServicesRepository reportingServicesRepository)
        {
            this._reportingServicesRepository = reportingServicesRepository;
        }

        #endregion

        #region Public Methods

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Reports()
        {
            return View();
        }

        public ActionResult ListReports()
        {
            return View();
        }

        [HttpPost]
        public string RunReport(RunSsrsReportModel reportParams)
        {
            try
            {
                reportParams = _reportingServicesRepository.RunReport(reportParams);

                var sessionKey = Guid.NewGuid().ToString();

                switch ((reportParams.Format ?? string.Empty).ToLower())
                {
                    case "xls":
                    case "pdf":
                        Session[string.Format("{0}_Name", sessionKey)] = reportParams.ReportName;
                        Session[sessionKey] = reportParams.ReportData;
                        return string.Format("url:/plugins/reportingservices/ViewReportExported/{0}/{1}",
                            reportParams.Export ? reportParams.Format :string.Format("{0}_inline", reportParams.Format), 
                            sessionKey);
                    default: // Assumed HTML
                        return "Invalid format";
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return ex.ToString();
            }
        }

        public FileResult ViewReportExported(string format, string key)
        {
            if (string.IsNullOrWhiteSpace(key) || Session[key] == null)
            {
                HttpContext.Response.StatusCode = 404;
                throw new Exception("We encountered an error while retrieving your report.  Please try again later");
            }

            var exportData = (byte[])Session[key];
            var fileExt = string.Empty;
            var appExt = string.Empty;
            var disp = string.Empty;
            switch (format.ToLower())
            {
                case "xls":
                    fileExt = ".xls";
                    appExt = "application/vnd.ms-excel";
                    disp = "attachment";
                    break;
                case "xls_inline":
                    fileExt = ".xls";
                    appExt = "application/vnd.ms-excel";
                    disp = "inline";
                    break;
                case "pdf":
                    fileExt = ".pdf";
                    appExt = "application/pdf";
                    disp = "attachment";
                    break;
                case "pdf_inline":
                    fileExt = ".pdf";
                    appExt = "application/pdf";
                    disp = "inline";
                    break;
            }

            var fileName = string.Format("{0}{1}",
                Session[string.Format("{0}_Name", key)] != null ? Session[string.Format("{0}_Name", key)].ToString() : "Exported_Report", fileExt);

            Session.Remove(key);

            MemoryStream output = new MemoryStream();
            output.Write(exportData, 0, exportData.Length);
            output.Position = 0;

            Response.AppendHeader("content-disposition", string.Format("{0}; filename={1}", disp, fileName));
            //Response.AppendHeader("content-disposition", "attachment; filename=file.pdf");

            return new FileStreamResult(output, appExt);
        }

        [System.Web.Http.HttpGet]
        public ActionResult SetReport(string reportPath)
        {
            Session["ReportPath"] = reportPath;
            return null;
        }

        public override PartialViewResult GetPartial(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                Response.StatusCode = 404;
                Response.StatusDescription = "Not Found - Invalid Path";
            }

            if (path[0] == '/')
                path = path.Substring(1, path.Length - 1);
            
            path = string.Format("~/Plugins/Axis.Plugins.ReportingServices/Views/{0}.cshtml", path.Replace(".cshtml", string.Empty));

            return PartialView(path);
        }

        #endregion
    }
}
