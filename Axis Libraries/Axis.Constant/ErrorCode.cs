namespace Axis.Constant
{
    /// <summary>
    /// This provides Error Codes for the Authentication Failure. 
    /// Note: This do not serve major purpose. We should consider moving them all in the SPs.
    /// </summary>
    public enum ErrorCode
    {
        InActive = -1,
        MaxLoginAttempts = -2,
        EffectiveDateError = -3,
        NoRoleAssigned = -4,
        ADCommFail = -5,
        ADWrongGroup = -6,
        ADAuthFailed = -7
    }
}
