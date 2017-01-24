using Axis.Service.Reporting;

namespace Axis.RuleEngine.Reporting
{
    /// <summary>
    ///
    /// </summary>
    public class ReportingRuleEngine : IReportingRuleEngine
    {
        #region Class Variables

        private IReportingService _reportingServiceService;

        #endregion

        #region Constructors

        public ReportingRuleEngine(IReportingService reportingService)
        {
            _reportingServiceService = reportingService;
        }

        #endregion

        #region Public Methods


        #endregion

    }
}