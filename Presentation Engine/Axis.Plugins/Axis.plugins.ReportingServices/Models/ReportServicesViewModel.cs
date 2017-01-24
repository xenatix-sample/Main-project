using System;
using Axis.PresentationEngine.Helpers.Model;

namespace Axis.Plugins.ReportingServices.Models
{
    public class ReportServicesViewModel : BaseViewModel
    {

        public int ReportID { get; set; }

        public int ReportTypeID { get; set; }

        public string ReportName { get; set; }

        public byte[] ReportModel { get; set; }

        public string ReportURL { get; set; }

        public int ReportGroupID { get; set; }

        public string ReportTypeName { get; set; }

        public string ReportDisplayName { get; set; }
    }
}
