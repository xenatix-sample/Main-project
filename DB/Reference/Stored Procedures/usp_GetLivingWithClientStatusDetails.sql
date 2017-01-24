-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetLivingWithClientStatusDetails]
-- Author:		Sumana Sangapu
-- Date:		09/10/2015
--
-- Purpose:		Gets the list of Living with Client Status lookup details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/10/2015	Sumana Sangapu		TFS# 2258 - Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetLivingWithClientStatusDetails]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		LivingWithClientStatusID, LivingWithClientStatus
		FROM		[Reference].[LivingWithClientStatus] 
		WHERE		IsActive = 1
		ORDER BY	LivingWithClientStatus ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END


GO


