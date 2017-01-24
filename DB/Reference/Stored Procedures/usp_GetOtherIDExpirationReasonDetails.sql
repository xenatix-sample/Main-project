-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetOtherIDExpirationReasonDetails]
-- Author:		Deepak Kumar
-- Date:		07/27/2016
--
-- Purpose:		Gets the list of Expiration Reason lookup details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/27/2016	Deepak Kumar	TFS# 11337 - Initial creation.
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [Reference].[usp_GetOtherIDExpirationReasonDetails]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		ExpirationReasonID, ExpirationReason 
		FROM		[Reference].[OtherIDExpirationReasons] 
		WHERE		IsActive = 1
		ORDER BY	ExpirationReason  ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END



GO


