using System;
using System.Collections.Generic;

namespace Axis.Model.ReportingServices
{
    public class SsrsBaseInfo
    {
        public string ID { get; set; }

        public string Description { get; set; }

        public string Path { get; set; }

        public string VirtualPath { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool HiddenSpecified { get; set; }
    }

    public class SsrsFolderInfo : SsrsBaseInfo
    {
        public string FolderName { get; set; }

        public IList<SsrsFolderInfo> SubFolders { get; set; }

        public IList<SsrsReportInfo> Reports { get; set; }

        public SsrsFolderInfo()
        {
            this.SubFolders = new List<SsrsFolderInfo>();
            this.Reports = new List<SsrsReportInfo>();
        }
    }

    public class SsrsReportInfo : SsrsBaseInfo
    {
        public int ReportID { get; set; }

        public int ReportGroupID { get; set; }

        public string ReportGroup { get; set; }

        public string ReportName { get; set; }

        public int? Size { get; set; }
    }

    public class SsrsReportParam
    {
        public string Name { get; set; }

        public string ParameterTypeName { get; set; }

        public bool Nullable { get; set; }

        public bool AllowBlank { get; set; }

        public bool MultiValue { get; set; }

        public bool QueryParameter { get; set; }

        public string Prompt { get; set; }

        public bool PromptUser { get; set; }

        public bool ValidValuesQueryBased { get; set; }

        public bool DefaultValuesQueryBased { get; set; }

        public string ParameterStateName { get; set; }

        public string ErrorMessage { get; set; }

        public bool Required { get { return !AllowBlank && (DefaultValues == null || DefaultValues.Count < 1); } }

        private string _Value { get; set; }
        public string Value
        {
            get
            {
                return !string.IsNullOrWhiteSpace(_Value) ? _Value :
                    (DefaultValues != null && DefaultValues.Count > 0 ? DefaultValues[0].Value : string.Empty);
            }
            set { _Value = value; }
        }

        public IList<string> Dependencies { get; set; }

        public IList<ValidValue> ValidValues { get; set; }

        public IList<ValidValue> DefaultValues { get; set; }

        private IList<ValidValue> _Values { get; set; }
        public IList<ValidValue> Values
        {
            get
            {
                if (Required)
                    return _Values ?? new List<ValidValue>();
                else
                    return _Values == null || _Values.Count < 1 ? DefaultValues ?? new List<ValidValue>() : _Values;
            }
            set { _Values = value; }
        }

        public SsrsReportParam()
        {
            _Values = new List<ValidValue>();
            DefaultValues = new List<ValidValue>();
            ValidValues = new List<ValidValue>();
            Dependencies = new List<string>();
        }
    }

    public class ValidValue
    {
        public string Label { get; set; }

        public string Value { get; set; }
    }

    public class SsrsParamValue
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public string Label { get; set; }
    }

    public class RunSsrsReportModel
    {
        public string ReportId { get; set; }

        public string ReportName { get; set; }

        public string Format { get; set; }

        public bool Export { get; set; }

        public IList<SsrsReportParam> ReportParameters { get; set; }

        public IList<SsrsParamValue> ParamValues { get; set; }

        public byte[] ReportData { get; set; }

        public RunSsrsReportModel()
        {
            ReportParameters = new List<SsrsReportParam>();
            ParamValues = new List<SsrsParamValue>();
        }
    }
}
