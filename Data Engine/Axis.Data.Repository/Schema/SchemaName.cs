namespace Axis.Data.Repository.Schema
{
    public enum SchemaName
    {
        [SchemaName("dbo")]
        Default = 0,
        [SchemaName("Core")]
        Core,
        [SchemaName("Reference")]
        Reference,
        [SchemaName("Registration")]
        Registration,
        [SchemaName("ESignature")]
        ESignature,
        [SchemaName("Scheduling")]
        Scheduling,
        [SchemaName("ECI")]
        ECI,
        [SchemaName("Clinical")]
        Clinical,
        [SchemaName("CallCenter")]
        CallCenter,
        [SchemaName("Synch")]
        Synch,
        [SchemaName("Auditing")]
        Auditing
    }
}
