using System;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.ReportingServices;
using Axis.Security;
using SSRS_Exec = Axis.Services.Reporting.SSRS_Exec;
using SSRS_SOAP = Axis.Services.Reporting.SSRS_SOAP;
using Axis.Service.Logging;
using Axis.Model.Logging;

namespace Axis.Service.Reporting
{
    /// <summary>
    ///
    /// </summary>
    public class ReportingService : IReportingService
    {
        #region Class Variables

        private readonly CommunicationManager _communicationManager;
        private const string BaseRoute = "ReportingServices/";

        private ILoggingService _Logger;

        private SSRS_Exec.ReportExecutionService _rs_Svc_2005;
        private SSRS_SOAP.ReportingService2010 _rs_Svc_2010;

        #endregion

        #region Constructors

        public ReportingService(ILoggingService logger)
        {
            _Logger = logger;
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);

            _rs_Svc_2005 = new SSRS_Exec.ReportExecutionService();
            _rs_Svc_2010 = new SSRS_SOAP.ReportingService2010();

            _rs_Svc_2005.Credentials = new Credentials()
            {
                Username = ConfigurationManager.AppSettings.Get("SSRS_Username"),
                Password = ConfigurationManager.AppSettings.Get("SSRS_Password"),
                Authority = ConfigurationManager.AppSettings.Get("SSRS_Authority")
            };

            _rs_Svc_2010.UseDefaultCredentials = false;
            _rs_Svc_2010.Credentials = new NetworkCredential()
            {
                UserName = ConfigurationManager.AppSettings.Get("NetAuth_Username"),
                Password = ConfigurationManager.AppSettings.Get("NetAuth_Password"),
                Domain = ConfigurationManager.AppSettings.Get("NetAuth_Domain")
            };
            _rs_Svc_2010.PreAuthenticate = true;
        }

        #endregion

        #region Public Methods

        public Response<SsrsFolderInfo> GetAllReports()
        {
            var rootFolder = ConfigurationManager.AppSettings.Get("SSRS_RootDir");

            try
            {
                var dbRepRes = GetReportsByType("ReportingServices");
                if (dbRepRes == null || dbRepRes.DataItems == null || dbRepRes.DataItems.Count < 1)
                    throw new Exception("Error while loading reports or no reports in database.");

                var response = new Response<SsrsFolderInfo>();
                var folders = new List<SsrsFolderInfo>();
                var items = _rs_Svc_2010.ListChildren(rootFolder, true);

                if (rootFolder.Length > 1)
                {
                    folders.Add(new SsrsFolderInfo()
                    {
                        FolderName = rootFolder.Substring(1),
                        Path = rootFolder,
                        HiddenSpecified = false
                    });
                }

                SsrsReportInfo report;
                ReportingServicesModel dbReport;
                foreach (SSRS_SOAP.CatalogItem ci in items)
                {
                    SsrsFolderInfo folder = null;

                    if (folders.Count > 0)
                        folder = _GetParentFolder(ref folders, ci);

                    switch (ci.TypeName.ToLower())
                    {
                        case "report":
                            if (folder == null)
                            {
                                continue;
                            }

                            dbReport = dbRepRes.DataItems
                                .FirstOrDefault(r => r.ReportName == ci.Name);

                            report = new SsrsReportInfo()
                            {
                                ID = ci.ID,
                                ReportName = ci.Name,
                                Description = ci.Description,
                                Path = ci.Path,
                                Size = ci.Size,
                                VirtualPath = ci.VirtualPath,
                                CreatedBy = ci.CreatedBy,
                                ModifiedBy = ci.ModifiedBy,
                                CreationDate = ci.CreationDate,
                                ModifiedDate = ci.ModifiedDate,
                                HiddenSpecified = ci.HiddenSpecified
                            };

                            if (dbReport != null)
                            {
                                report.ReportID = dbReport.ReportID;
                                report.ReportGroupID = dbReport.ReportGroupID;
                                report.ReportGroup = dbReport.ReportGroup;
                            }

                            folder.Reports.Add(report);
                            break;
                        case "folder":
                            if (folder == null)
                            {
                                folders.Add(new SsrsFolderInfo()
                                {
                                    ID = ci.ID,
                                    FolderName = ci.Name,
                                    Description = ci.Description,
                                    Path = ci.Path,
                                    VirtualPath = ci.VirtualPath,
                                    CreatedBy = ci.CreatedBy,
                                    ModifiedBy = ci.ModifiedBy,
                                    CreationDate = ci.CreationDate,
                                    ModifiedDate = ci.ModifiedDate,
                                    HiddenSpecified = ci.HiddenSpecified
                                });
                            }
                            else
                            {
                                folder.SubFolders.Add(new SsrsFolderInfo()
                                {
                                    ID = ci.ID,
                                    FolderName = ci.Name,
                                    Description = ci.Description,
                                    Path = ci.Path,
                                    VirtualPath = ci.VirtualPath,
                                    CreatedBy = ci.CreatedBy,
                                    ModifiedBy = ci.ModifiedBy,
                                    CreationDate = ci.CreationDate,
                                    ModifiedDate = ci.ModifiedDate,
                                    HiddenSpecified = ci.HiddenSpecified
                                });
                            }
                            break;
                    }
                }

                response.DataItems = folders;

                return response;
            }
            catch (Exception ex)
            {
                _Logger.LogException(new ExceptionModel()
                {
                    Message = ex.ToString(),
                    Source = "Axis.Service.Reporting.ReportingService.GetAllReports"
                });
                throw ex;
            }
        }

        public Response<SsrsReportInfo> GetReportByID(string reportId)
        {
            try
            {
                var response = new Response<SsrsReportInfo>();
                response.DataItems.Add(_GetReportByID(reportId));
                return response;
            }
            catch (Exception ex)
            {
                _Logger.LogException(new ExceptionModel()
                {
                    Message = ex.ToString(),
                    Comments = "reportId" + reportId,
                    Source = "Axis.Service.Reporting.ReportingService.GetReportByID"
                });
                throw ex;
            }
        }

        public Response<SsrsReportParam> LoadReportParams(RunSsrsReportModel reportParams)
        {
            try
            {
                var response = new Response<SsrsReportParam>();
                response.DataItems = new List<SsrsReportParam>();
                var report = _GetReportByID(reportParams.ReportId);

                SSRS_SOAP.ParameterValue[] pValues = null;
                SSRS_SOAP.DataSourceCredentials[] credentials = null;

                if (reportParams.ParamValues != null && reportParams.ParamValues.Count > 0)
                {
                    // Get UserID param
                    var pUserID = reportParams.ParamValues
                        .FirstOrDefault(p => p.Name == "ReportUserID");

                    // Set UserID param
                    // For data assurance, UserID needs to be explicitly set even if passed from UI
                    if (pUserID != null)
                        pUserID.Value = AuthContext.Auth.User.UserID.ToString();

                    pValues = reportParams.ParamValues
                        .Where(v => v.Value != null)
                        .Select(v => new SSRS_SOAP.ParameterValue()
                        {
                            Name = v.Name,
                            Value = v.Value,
                            Label = v.Label,
                        }).ToArray();
                }

                var parameters = _rs_Svc_2010.GetItemParameters(report.Path, null, true, pValues, credentials);

                foreach (SSRS_SOAP.ItemParameter v in parameters)
                {
                    var p = new SsrsReportParam()
                    {
                        Name = v.Name,
                        ParameterTypeName = v.ParameterTypeName,
                        Nullable = v.Nullable,
                        AllowBlank = v.AllowBlank,
                        MultiValue = v.MultiValue,
                        QueryParameter = v.QueryParameter,
                        Prompt = v.Prompt,
                        PromptUser = v.PromptUser,
                        ValidValuesQueryBased = v.ValidValuesQueryBased,
                        DefaultValuesQueryBased = v.DefaultValuesQueryBased,
                        ParameterStateName = v.ParameterStateName,
                        ErrorMessage = v.ErrorMessage
                    };

                    if (p.Name == "ReportUserID")
                        p.Value = AuthContext.Auth.User.UserID.ToString();

                    if (v.Dependencies != null)
                        p.Dependencies = v.Dependencies.ToList();

                    if (v.ValidValues != null)
                        p.ValidValues = v.ValidValues.Select(vv => new ValidValue()
                        {
                            Label = vv.Label,
                            Value = vv.Value
                        }).ToList();

                    ValidValue defVal;
                    if (v.DefaultValues != null)
                    {
                        foreach (var dv in v.DefaultValues.Distinct())
                        {
                            defVal = p.ValidValues.FirstOrDefault(vv => vv.Value == dv);

                            if (defVal != null)
                                p.DefaultValues.Add(defVal);
                        }
                    }

                    response.DataItems.Add(p);
                }

                return response;
            }
            catch (Exception ex)
            {
                _Logger.LogException(new ExceptionModel()
                {
                    Message = ex.ToString(),
                    Comments = "reportId" + reportParams.ReportId,
                    Source = "Axis.Service.Reporting.ReportingService.LoadReportParams"
                });
                throw ex;
            }
        }

        public RunSsrsReportModel RunReport(RunSsrsReportModel reportParams)
        {
            try
            {
                var response = new RunSsrsReportModel();

                var outputFormat = string.Empty;
                switch ((reportParams.Format ?? string.Empty).ToLower())
                {
                    case "html":
                        outputFormat = "HTML4.0";
                        break;
                    case "xls":
                        outputFormat = "EXCEL";
                        break;
                    case "word":
                        outputFormat = "WORD";
                        break;
                    case "pdf":
                        outputFormat = "PDF";
                        break;
                    default:
                        outputFormat = "HTML4.0";
                        break;
                }

                string deviceInfo = "<DeviceInfo><Toolbar>False</Toolbar></DeviceInfo>";
                string extension;
                string encoding;
                string mimeType;
                SSRS_Exec.Warning[] warnings;
                string[] streamIDs;

                var report = _GetReportByID(reportParams.ReportId);
                var reportInfo = _rs_Svc_2005.LoadReport(report.Path, null);

                // Extract Param Values
                var parameters = new List<SSRS_Exec.ParameterValue>();
                foreach (var ssrsparam in reportParams.ReportParameters)
                {
                    if (ssrsparam.MultiValue)
                    {
                        foreach (var val in ssrsparam.Values)
                        {
                            parameters.Add(new SSRS_Exec.ParameterValue()
                            {
                                Name = ssrsparam.Name,
                                Value = val.Value
                            });
                        }
                    }
                    else if (ssrsparam.ParameterTypeName == "DateTime")
                    {
                        parameters.Add(new SSRS_Exec.ParameterValue()
                        {
                            Name = ssrsparam.Name,
                            Value = Convert.ToDateTime(ssrsparam.Value).ToString("dd/MM/yyyy")
                        });
                    }
                    else
                    {
                        parameters.Add(new SSRS_Exec.ParameterValue()
                        {
                            Name = ssrsparam.Name,
                            Value = ssrsparam.Value
                        });
                    }
                }

                var manParamNames = new string[] { "ReportUserID", "ReportName", "ReportUserOrgStructure" };
                foreach (var param in parameters.Where(p => manParamNames.Contains(p.Name)))
                {
                    switch (param.Name)
                    {
                        case "ReportUserID":
                            param.Value = AuthContext.Auth.User.UserID.ToString();
                            break;
                        case "ReportName":
                            param.Value = report.ReportName;
                            break;
                        case "ReportUserOrgStructure":
                            param.Value = report.ReportGroupID.ToString();
                            break;
                    }
                }

                _rs_Svc_2005.SetExecutionParameters(parameters.ToArray(), "en-us");

                reportParams.ReportName = report.ReportName;
                reportParams.ReportData = _rs_Svc_2005.Render(outputFormat, deviceInfo, out extension, out mimeType, out encoding, out warnings, out streamIDs);

                return reportParams;
            }
            catch (Exception ex)
            {
                _Logger.LogException(new ExceptionModel()
                {
                    Message = ex.ToString(),
                    Comments = "reportId" + reportParams.ReportId,
                    Source = "Axis.Service.Reporting.ReportingService.RunReport"
                });
                throw ex;
            }
        }

        public Response<ReportingServicesModel> GetReportsByType(string reportTypeName)
        {
            var apiUrl = BaseRoute + "GetReportsByType";
            var param = new NameValueCollection { { "ReportTypeName", reportTypeName } };
            return _communicationManager.Get<Response<ReportingServicesModel>>(param, apiUrl);
        }

        #endregion

        #region Private Methods

        private SsrsFolderInfo _GetParentFolder(ref List<SsrsFolderInfo> folders, SSRS_SOAP.CatalogItem item)
        {
            SsrsFolderInfo folder = null;
            var parentPath = item.Path.Substring(0, item.Path.LastIndexOf(item.Name));
            parentPath = parentPath.TrimEnd('/');

            if (parentPath.Length < 1)
                return null;
            else
            {
                var strings = parentPath.IndexOf('/') == 0 ? parentPath.Substring(1).Split('/') : parentPath.Split('/');

                foreach (var str in strings)
                {
                    SsrsFolderInfo tmpF = null;

                    if (folder == null)
                        tmpF = folders.FirstOrDefault(f => f.FolderName == str);
                    else
                        tmpF = folder.SubFolders.FirstOrDefault(f => f.FolderName == str);

                    if (tmpF != null)
                        folder = tmpF;
                    else
                    {
                        return null;
                    }
                }
            }

            return folder;
        }

        private SsrsReportInfo _GetReportByID(string reportId)
        {
            var reports = new Response<SsrsReportInfo>();
            var items = _rs_Svc_2010.ListChildren(ConfigurationManager.AppSettings.Get("SSRS_RootDir"), true);

            var report = items.Select(r => new SsrsReportInfo()
            {
                ID = r.ID,
                ReportName = r.Name,
                Description = r.Description,
                Path = r.Path,
                Size = r.Size,
                VirtualPath = r.VirtualPath,
                CreatedBy = r.CreatedBy,
                ModifiedBy = r.ModifiedBy,
                CreationDate = r.CreationDate,
                ModifiedDate = r.ModifiedDate,
                HiddenSpecified = r.HiddenSpecified
            })
            .FirstOrDefault(r => r.ID == reportId);

            var dbRepRes = GetReportsByType("ReportingServices");
            var dbReport = dbRepRes.DataItems.FirstOrDefault(r => r.ReportName == report.ReportName);

            report.ReportID = dbReport.ReportID;
            report.ReportGroupID = dbReport.ReportGroupID;
            report.ReportGroup = dbReport.ReportGroup;

            return report;
        }

        #endregion

        //public Response<SsrsFolderInfo> GetAllReports()
        //{
        //    var apiUrl = BaseRoute + "GetAllReports";
        //    return _communicationManager.Get<Response<SsrsFolderInfo>>(apiUrl);
        //}

        //public Response<SsrsReportInfo> GetReportByID(string reportId)
        //{
        //    var apiUrl = BaseRoute + "GetReportByID";
        //    var param = new NameValueCollection { { "reportId", reportId } };
        //    return _communicationManager.Get<Response<SsrsReportInfo>>(param, apiUrl);
        //}

        //public Response<SsrsReportParam> LoadReportParams(RunSsrsReportModel reportParams)
        //{
        //    var apiUrl = BaseRoute + "LoadReportParams";
        //    return _communicationManager.Post<RunSsrsReportModel, Response<SsrsReportParam>>(reportParams, apiUrl);
        //}

        //public RunSsrsReportModel RunReport(RunSsrsReportModel reportParams)
        //{
        //    var apiUrl = BaseRoute + "RunReport";
        //    return _communicationManager.Post<RunSsrsReportModel, RunSsrsReportModel>(reportParams, apiUrl);
        //}
    }
}