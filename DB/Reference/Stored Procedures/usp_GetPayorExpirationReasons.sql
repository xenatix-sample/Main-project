-----------------------------------------------------------------------------------------------------------------------
-- Procedure:   [usp_GetPayorExpirationReasons]
-- Author:		Sumana Sangapu
-- Date:		03/16/2016
--
-- Purpose:		Gets the list of Payor Eligibility Expiration Reasons 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------

-- 03/16/2016	Kyle Campbell	TFS #7308	Initial creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetPayorExpirationReasons]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		PayorExpirationReasonID, PayorExpirationReason
		FROM		[Reference].[PayorExpirationReason] 
		WHERE		IsActive = 1
		ORDER BY	SortOrder  ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END


GO

