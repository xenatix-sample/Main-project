-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_GetContactLettersByContactID]
-- Author:		Deepak Kumar
-- Date:		06/06/2016
--
-- Purpose:		Get a list of ContactLetters by contact
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/06/2016	Deepak Kumar	Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_GetContactLettersByContactID]
    @ContactID BIGINT,
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
		LEFT JOIN [Core].[Users] U
			ON II.UserID = u.UserID
	WHERE 
		II.IsActive = 1 
		AND II.ContactID = @ContactID
	ORDER BY
		II.ModifiedOn DESC
         
    END TRY
    BEGIN CATCH
            SELECT @ResultCode = ERROR_SEVERITY(),
                    @ResultMessage = ERROR_MESSAGE()
    END CATCH
END