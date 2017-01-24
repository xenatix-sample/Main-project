using System;
using System.Collections.Generic;

namespace Axis.Plugins.ECI.Model
{
    public class ScreeningViewModel : ECIBaseViewModel
    {
        public long? ScreeningID { get; set; }
        public int ProgramID { get; set; }
        public string ProgramName { get; set; }
        public DateTime InitialContactDate { get; set; }
        public int? InitialServiceCoordinatorID { get; set; }
        public string InitialServiceCoordinator { get; set; }
        public int? PrimaryServiceCoordinatorID { get; set; }
        public string PrimaryServiceCoordinator { get; set; }
        public DateTime ScreeningDate { get; set; }
        public Int16? ScreeningTypeID { get; set; }
        public string ScreeningType { get; set; }
        public long? AssessmentID { get; set; }
        public string AssessmentName { get; set; }
        public Int16? ScreeningResultsID { get; set; }
        public string ScreeningResult { get; set; }
        public int? ScreeningScore { get; set; }
        public Int16? ScreeningStatusID { get; set; }
        public string ScreeningStatus { get; set; }
        public int? SubmittedByID { get; set; }
        public string SubmittedBy { get; set; }
        public long? ResponseID { get; set; }
        public long? SectionID { get; set; }
    }
}