-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_GetBenefitsAssistance]
-- Author:		Scott Martin
-- Date:		05/19/2016
--
-- Purpose:		Get a list of benefits assistance by contact
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 05/19/2016	Scott Martin	Initial creation.
-- 05/27/2016	Scott Martin	Added Order by ModifiedOn DESC
-- 12/25/2016	Vishal Yadav	Added ServiceRecordingID in return object
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_GetBenefitsAssistance]
    @BenefitsAssistanceID BIGINT,
    @ResultCode INT OUTPUT,
    @ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
    BEGIN TRY
	SELECT @ResultCode = 0,
		@ResultMessage = 'executed successfully'

	SELECT 
		BA.BenefitsAssistanceID, 
		BA.ContactID,
		BA.DocumentStatusID,
		BA.DateEntered, 
		SR.UserID, 
		U.FirstName + ' ' + U.LastName as ProviderName,
		BA.AssessmentID, 
		BA.ResponseID,
		BA.ModifiedBy, 
		BA.ModifiedOn,
		CAST(CASE
			WHEN SRV.ServiceRecordingVoidID IS NOT NULL THEN 1
			ELSE 0 END AS BIT) AS IsVoided,
		SR.ServiceStartDate,
		SR.ServiceEndDate,
		SR.ServiceItemID,
		SR.TrackingFieldID,
		SR.ServiceRecordingID		
	FROM 
		[Registration].[BenefitsAssistance]  BA
		LEFT OUTER JOIN Core.ServiceRecording SR
			ON BA.BenefitsAssistanceID = SR.SourceHeaderID
			AND SR.ServiceRecordingSourceID = 4
		LEFT OUTER JOIN Core.ServiceRecordingVoid SRV
			ON SR.ServiceRecordingID = SRV.ServiceRecordingID
		LEFT JOIN [Core].[Users] U
			ON SR.UserID = u.UserID
	WHERE 
		BA.IsActive = 1 
		AND BA.BenefitsAssistanceID = @BenefitsAssistanceID
	ORDER BY
		BA.ModifiedOn DESC
         
    END TRY
    BEGIN CATCH
            SELECT @ResultCode = ERROR_SEVERITY(),
                    @ResultMessage = ERROR_MESSAGE()
    END CATCH
END
GO