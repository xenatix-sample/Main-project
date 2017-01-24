-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetExpirationReasonDetails]
-- Author:		Sumana Sangapu
-- Date:		07/22/2015
--
-- Purpose:		Gets the list of Expiration Reason lookup details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/22/2015	Sumana Sangapu		TFS# 674 - Initial creation.
-- 07/30/2015   Sumana Sangapu	1016 - Changed schema from dbo to Registration/Core/Reference
-- 08/03/2015   Sumana Sangapu	1016 - Changed proc name schema qualifier from dbo to Registration/Core/Reference
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [Reference].[usp_GetExpirationReasonDetails]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		ExpirationReasonID, ExpirationReason 
		FROM		[Reference].[ExpirationReason] 
		WHERE		IsActive = 1
		ORDER BY	ExpirationReason  ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END



GO


