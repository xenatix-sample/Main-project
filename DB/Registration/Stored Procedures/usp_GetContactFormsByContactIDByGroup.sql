-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_GetContactFormsByContactIDByGroup]
-- Author:		Scott Martin
-- Date:		06/10/2016
--
-- Purpose:		Get a list of ContactForms by contact by Group
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/10/2016	Scott Martin	Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_GetContactFormsByContactIDByGroup]
    @ContactID BIGINT,
	@AssessmentGroupID BIGINT,
    @ResultCode INT OUTPUT,
    @ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
    BEGIN TRY
	SELECT @ResultCode = 0,
		@ResultMessage = 'executed successfully'

	SELECT 
		II.ContactFormsID, 
		II.ContactID, 
		SR.UserID, 
		U.FirstName + ' ' + U.LastName as ProviderName,
		II.AssessmentID, 
		II.ResponseID,
		II.DocumentStatusID,
		II.ModifiedBy, 
		II.ModifiedOn,
		CAST(CASE
			WHEN SRV.ServiceRecordingVoidID IS NOT NULL THEN 1
			ELSE 0 END AS BIT) AS IsVoided,
		SR.ServiceStartDate,
		SR.ServiceEndDate
	FROM 
		[Registration].[ContactForms]  II
		LEFT OUTER JOIN Core.AssessmentGroupDetails AGD
			ON II.AssessmentID = AGD.AssessmentID
		LEFT OUTER JOIN Core.AssessmentGroup AG
			ON AGD.AssessmentGroupID = AG.AssessmentGroupID
		LEFT OUTER JOIN Core.ServiceRecording SR
			ON II.ContactFormsID = SR.SourceHeaderID
			AND SR.ServiceRecordingSourceID = 5
		LEFT OUTER JOIN Core.ServiceRecordingVoid SRV
			ON SR.ServiceRecordingID = SRV.ServiceRecordingID
		LEFT OUTER JOIN [Core].[Users] U
			ON SR.UserID = u.UserID
	WHERE 
		II.IsActive = 1 
		AND II.ContactID = @ContactID
		AND AG.AssessmentGroupID = @AssessmentGroupID
	ORDER BY
		II.ModifiedOn DESC
         
    END TRY
    BEGIN CATCH
            SELECT @ResultCode = ERROR_SEVERITY(),
                    @ResultMessage = ERROR_MESSAGE()
    END CATCH
END