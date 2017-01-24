-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_GetContactLettersByContactIDByGroup]
-- Author:		Scott Martin
-- Date:		06/06/2016
--
-- Purpose:		Get a list of ContactLetters by contact by Group
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/08/2016	Scott Martin	Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_GetContactLettersByContactIDByGroup]
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
		II.ContactLettersID, 
		II.ContactID, 
		II.SentDate, 
		II.UserID, 
		U.FirstName + ' ' + U.LastName as ProviderName,
		II.AssessmentID, 
		II.ResponseID,
		II.LetterOutcomeID,
		II.Comments,
		II.ModifiedBy, 
		II.ModifiedOn
	FROM 
		[Registration].[ContactLetters]  II
		LEFT OUTER JOIN [Core].[Users] U
			ON II.UserID = u.UserID
		LEFT OUTER JOIN Core.AssessmentGroupDetails AGD
			ON II.AssessmentID = AGD.AssessmentID
		LEFT OUTER JOIN Core.AssessmentGroup AG
			ON AGD.AssessmentGroupID = AG.AssessmentGroupID
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