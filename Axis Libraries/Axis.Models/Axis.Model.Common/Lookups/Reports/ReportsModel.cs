namespace Axis.Model.Common
{
    public class ReportsModel : BaseEntity
    {
        public int ReportID { get; set; }
        public int ReportTypeID { get; set; }
        public string ReportTypeName { get; set; }
        public string ReportName { get; set; }
        public string ReportDisplayName { get; set; }
        public byte[] ReportModel { get; set; }
    }
}
