namespace Axis.Model.Common
{
    public class ExpirationReasonModel : BaseEntity
    {
        public int ExpirationReasonID { get; set; }
        public string ExpirationReason { get; set; }
        public int AssessmentExpirationReasonID { get; set; }
        public string AssessmentExpirationReason { get; set; }
    }
}
