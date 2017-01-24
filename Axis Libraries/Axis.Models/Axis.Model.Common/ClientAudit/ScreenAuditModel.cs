using System;
namespace Axis.Model.Common.ClientAudit
{
    /// <summary>
    /// Represents Client Audit
    /// </summary>
    public class ScreenAuditModel : BaseEntity
    {
        public int UserID { get; set; }
        public long? TransactionLogID { get; set; }
        public int? ContactID { get; set; }
        public string DataKey { get; set; }
        public int ActionTypeID { get; set; }
        public bool? IsCareMember { get; set; }
        public bool? IsBreaktheGlassEnabled { get; set; }
        public string SearchText { get; set; }
    }
}
