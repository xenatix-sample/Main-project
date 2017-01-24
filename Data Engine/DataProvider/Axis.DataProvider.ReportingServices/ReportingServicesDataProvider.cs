using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.ReportingServices;
using Axis.Security;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.ReportingServices
{
    public class ReportingServicesDataProvider : IReportingServicesDataProvider
    {
        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CallCenterProgressNoteDataProvider" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ReportingServicesDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Collects the reports from reporting Services based on report type Name.
        /// </summary>
        /// <param name="reportTypeName"></param>
        /// <returns></returns>
        public Response<ReportingServicesModel> GetReportsByType(string reportTypeName)
        {
            Response<ReportingServicesModel> returnReportingServicesModel = null;
            try
            {
                var reportingServivesRepository = _unitOfWork.GetRepository<ReportingServicesModel>(SchemaName.Core);
                var procParams = new List<SqlParameter>
                {
                    new SqlParameter("ReportTypeName", reportTypeName),
                    new SqlParameter("UserID", AuthContext.Auth.User.UserID )
                };
                returnReportingServicesModel = reportingServivesRepository.ExecuteStoredProc("usp_GetReportsByType", procParams);

                if (returnReportingServicesModel.DataItems.Count <= 0)
                {
                    System.Diagnostics.Debug.WriteLine("Reporting Services Executed Successfully : usp_GetReportsByType -- No Data Found for " + reportTypeName);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Reporting Services Error: usp_GetReportsByType" + "reportTypeName" + reportTypeName + ex.InnerException);
            }

            return returnReportingServicesModel;
        }

        /// <summary>
        /// Collect the reporting groups based on report group ID.
        /// </summary>
        /// <param name="reportGroupID"></param>
        /// <returns></returns>
        public Response<ReportingServicesGroupModel> GetReportGroupDetail(int reportGroupID)
        {
            Response<ReportingServicesGroupModel> returnReportingServicesModel = null;

            try
            {
                var reportingServivesGroupRepository = _unitOfWork.GetRepository<ReportingServicesGroupModel>(SchemaName.Reference);
                var procParams = new List<SqlParameter> { new SqlParameter("ReportTypeName", reportGroupID) };
                returnReportingServicesModel = reportingServivesGroupRepository.ExecuteStoredProc("usp_GetReportGroupDetails", procParams);

                if (returnReportingServicesModel.DataItems.Count <= 0)
                {
                    System.Diagnostics.Debug.WriteLine("Reporting Services Executed Successfully :usp_GetReportGroupDetails -- No Data Found for usp_GetReportGroupDetails" + "GroupId" + reportGroupID);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Reporting Services Error: usp_GetReportGroupDetails" + "Report GroupID:" + reportGroupID + ex.InnerException);
            }

            return returnReportingServicesModel;
        }

        ///// <summary>
        ///// Collects the reports from reporting Services based on report type Name.
        ///// </summary>
        ///// <returns></returns>
        //public Response<SsrsFolderInfo> GetAllReports()
        //{
        //    var rootFolder = ConfigurationSettings.AppSettings.Get("SSRS_RootDir");

        //    try
        //    {
        //        var response = new Response<SsrsFolderInfo>();
        //        var folders = new List<SsrsFolderInfo>();
        //        var items = SVC.ListChildren(rootFolder, true);

        //        if (rootFolder.Length > 1)
        //        {
        //            folders.Add(new SsrsFolderInfo()
        //            {
        //                FolderName = rootFolder.Substring(1),
        //                Path = rootFolder,
        //                HiddenSpecified = false
        //            });
        //        }

        //        foreach (CatalogItem ci in items)
        //        {
        //            SsrsFolderInfo folder = null;

        //            if (folders.Count > 0)
        //                folder = _GetParentFolder(ref folders, ci);

        //            switch (ci.TypeName.ToLower())
        //            {
        //                case "report":
        //                    if (folder == null)
        //                    {
        //                        continue;
        //                    }

        //                    folder.Reports.Add(new SsrsReportInfo()
        //                    {
        //                        ID = ci.ID,
        //                        FileName = ci.Name,
        //                        Description = ci.Description,
        //                        Path = ci.Path,
        //                        Size = ci.Size,
        //                        VirtualPath = ci.VirtualPath,
        //                        CreatedBy = ci.CreatedBy,
        //                        ModifiedBy = ci.ModifiedBy,
        //                        CreationDate = ci.CreationDate,
        //                        ModifiedDate = ci.ModifiedDate,
        //                        HiddenSpecified = ci.HiddenSpecified
        //                    });
        //                    break;
        //                case "folder":
        //                    if (folder == null)
        //                    {
        //                        folders.Add(new SsrsFolderInfo()
        //                        {
        //                            ID = ci.ID,
        //                            FolderName = ci.Name,
        //                            Description = ci.Description,
        //                            Path = ci.Path,
        //                            VirtualPath = ci.VirtualPath,
        //                            CreatedBy = ci.CreatedBy,
        //                            ModifiedBy = ci.ModifiedBy,
        //                            CreationDate = ci.CreationDate,
        //                            ModifiedDate = ci.ModifiedDate,
        //                            HiddenSpecified = ci.HiddenSpecified
        //                        });
        //                    }
        //                    else
        //                    {
        //                        folder.SubFolders.Add(new SsrsFolderInfo()
        //                        {
        //                            ID = ci.ID,
        //                            FolderName = ci.Name,
        //                            Description = ci.Description,
        //                            Path = ci.Path,
        //                            VirtualPath = ci.VirtualPath,
        //                            CreatedBy = ci.CreatedBy,
        //                            ModifiedBy = ci.ModifiedBy,
        //                            CreationDate = ci.CreationDate,
        //                            ModifiedDate = ci.ModifiedDate,
        //                            HiddenSpecified = ci.HiddenSpecified
        //                        });
        //                    }
        //                    break;
        //            }
        //        }

        //        response.DataItems = folders;

        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        ///// <summary>
        ///// Collects the reports from reporting Services based on report type Name.
        ///// </summary>
        ///// <returns></returns>
        //public Response<SsrsReportInfo> GetReportByID(string reportId)
        //{
        //    try
        //    {
        //        var response = new Response<SsrsReportInfo>();
        //        response.DataItems.Add(_GetReportByID(reportId));
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Reporting Services Error: GetReportByID" + "reportId" + reportId + " - " + ex.InnerException);
        //    }
        //}

        ///// <summary>
        ///// Gets the parameters for the specified report.
        ///// </summary>
        ///// <param name="reportId"></param>
        ///// <returns></returns>
        //public Response<SsrsReportParam> LoadReportParams(RunSsrsReportModel reportParams)
        //{
        //    try
        //    {
        //        var response = new Response<SsrsReportParam>();
        //        response.DataItems = new List<SsrsReportParam>();
        //        var report = _GetReportByID(reportParams.ReportId);

        //        SSRS_SOAP.ParameterValue[] pValues = null;
        //        SSRS_SOAP.DataSourceCredentials[] credentials = null;

        //        if (reportParams.ParamValues.Count > 0)
        //            pValues = reportParams.ParamValues.Select(v => new SSRS_SOAP.ParameterValue()
        //            {
        //                Name = v.Name,
        //                Value = v.Value,
        //                Label = v.Label,
        //            }).ToArray();

        //        var parameters = SVC.GetItemParameters(report.Path, null, true, pValues, credentials);

        //        foreach (SSRS_SOAP.ItemParameter v in parameters)
        //        {
        //            var p = new SsrsReportParam()
        //            {
        //                Name = v.Name,
        //                ParameterTypeName = v.ParameterTypeName,
        //                Nullable = v.Nullable,
        //                AllowBlank = v.AllowBlank,
        //                MultiValue = v.MultiValue,
        //                QueryParameter = v.QueryParameter,
        //                Prompt = v.Prompt,
        //                PromptUser = v.PromptUser,
        //                ValidValuesQueryBased = v.ValidValuesQueryBased,
        //                DefaultValuesQueryBased = v.DefaultValuesQueryBased,
        //                ParameterStateName = v.ParameterStateName,
        //                ErrorMessage = v.ErrorMessage
        //            };

        //            if (v.Dependencies != null)
        //                p.Dependencies = v.Dependencies.ToList();

        //            if (v.ValidValues != null)
        //                p.ValidValues = v.ValidValues.Select(vv => new Model.ReportingServices.ValidValue()
        //                {
        //                    Label = vv.Label,
        //                    Value = vv.Value
        //                }).ToList();

        //            if (v.DefaultValues != null)
        //                p.DefaultValues = v.DefaultValues.Select(dv => new Model.ReportingServices.ValidValue()
        //                {
        //                    Label = dv,
        //                    Value = dv
        //                }).ToList();

        //            response.DataItems.Add(p);
        //        }

        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Reporting Services Error: LoadReportParams" + "reportId" + reportParams.ReportId + " - " + ex.InnerException);
        //    }
        //}

        ///// <summary>
        ///// Collects the reports from reporting Services based on report type Name.
        ///// </summary>
        ///// <returns></returns>
        //public RunSsrsReportModel RunReport(RunSsrsReportModel reportParams)
        //{
        //    try
        //    {
        //        var response = new RunSsrsReportModel();
        //        var report = _GetReportByID(reportParams.ReportId);

        //        ReportExecutionService rs = new ReportExecutionService();
                
        //        rs.Credentials = new Credentials()
        //        {
        //            Username = ConfigurationSettings.AppSettings.Get("NetAuth_Username"),
        //            Password = ConfigurationSettings.AppSettings.Get("NetAuth_Password"),
        //            Authority = ConfigurationSettings.AppSettings.Get("NetAuth_Authority")
        //        };

        //        rs.LoadReport(report.Path, null);

        //        var outputFormat = string.Empty;
        //        switch ((reportParams.Format ?? string.Empty).ToLower())
        //        {
        //            case "html":
        //                outputFormat = "HTML4.0";
        //                break;
        //            case "xls":
        //                outputFormat = "EXCEL";
        //                break;
        //            case "word":
        //                outputFormat = "WORD";
        //                break;
        //            case "pdf":
        //                outputFormat = "PDF";
        //                break;
        //            default:
        //                outputFormat = "HTML4.0";
        //                break;
        //        }

        //        string deviceInfo = "<DeviceInfo><Toolbar>False</Toolbar></DeviceInfo>";
        //        byte[] result;
        //        string extension;
        //        string encoding;
        //        string mimeType;
        //        SSRS_Exec.Warning[] warnings;
        //        string[] streamIDs;

        //        if (reportParams.ReportParameters != null && reportParams.ReportParameters.Count > 0)
        //        {
        //            List<SSRS_Exec.ParameterValue> parameters = new List<SSRS_Exec.ParameterValue>();
        //            foreach (var ssrsparam in reportParams.ReportParameters)
        //            {
        //                if (ssrsparam.MultiValue)
        //                {
        //                    foreach (var val in ssrsparam.Values)
        //                    {
        //                        parameters.Add(new SSRS_Exec.ParameterValue()
        //                        {
        //                            Name = ssrsparam.Name,
        //                            Value = val.Value
        //                        });
        //                    }
        //                }
        //                else
        //                {
        //                    parameters.Add(new SSRS_Exec.ParameterValue()
        //                    {
        //                        Name = ssrsparam.Name,
        //                        Value = ssrsparam.Value
        //                    });
        //                }
        //            }

        //            rs.SetExecutionParameters(parameters.ToArray(), "en-us");
        //        }

        //        reportParams.ReportName = report.FileName;
        //        reportParams.ReportData = rs.Render(outputFormat, deviceInfo, out extension, out mimeType, out encoding, out warnings, out streamIDs);
                
        //        return reportParams;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Reporting Services Error: RunReport" + "reportId" + reportParams.ReportId + " - " + ex.InnerException);
        //    }
        //}

        //private SsrsFolderInfo _GetParentFolder(ref List<SsrsFolderInfo> folders, CatalogItem item)
        //{
        //    SsrsFolderInfo folder = null;
        //    var parentPath = item.Path.Substring(0, item.Path.LastIndexOf(item.Name));
        //    parentPath = parentPath.TrimEnd('/');

        //    if (parentPath.Length < 1)
        //        return null;
        //    else
        //    {
        //        var strings = parentPath.IndexOf('/') == 0 ? parentPath.Substring(1).Split('/') : parentPath.Split('/');

        //        foreach (var str in strings)
        //        {
        //            SsrsFolderInfo tmpF = null;

        //            if (folder == null)
        //                tmpF = folders.FirstOrDefault(f => f.FolderName == str);
        //            else
        //                tmpF = folder.SubFolders.FirstOrDefault(f => f.FolderName == str);

        //            if (tmpF != null)
        //                folder = tmpF;
        //            else
        //            {
        //                return null;
        //            }
        //        }
        //    }

        //    return folder;
        //}

        //private SsrsReportInfo _GetReportByID(string reportId)
        //{
        //    var reports = new Response<SsrsReportInfo>();
        //    var items = SVC.ListChildren(ConfigurationSettings.AppSettings.Get("SSRS_RootDir"), true);

        //    return items.Select(report => new SsrsReportInfo()
        //    {
        //        ID = report.ID,
        //        FileName = report.Name,
        //        Description = report.Description,
        //        Path = report.Path,
        //        Size = report.Size,
        //        VirtualPath = report.VirtualPath,
        //        CreatedBy = report.CreatedBy,
        //        ModifiedBy = report.ModifiedBy,
        //        CreationDate = report.CreationDate,
        //        ModifiedDate = report.ModifiedDate,
        //        HiddenSpecified = report.HiddenSpecified
        //    })
        //    .FirstOrDefault(report => report.ID == reportId);
        //}
    }
}