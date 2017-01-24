using System;
namespace Axis.Model.Common.ClientAudit
{
    /// <summary>
    /// Represents Client Audit
    /// </summary>
    public class ClientAuditModel : BaseEntity
    {
        public long AuditClientViewCodeID { get; set; }
        public string AuditKey { get; set; }
        public string AuditValue { get; set; }
    }
}
