
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:   [ECI].[usp_GetReasonForDelay]
-- Author:		John Crossen
-- Date:		09/03/2015
--
-- Purpose:		Gets the list of  ReasonForDelay lookup Details 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/03/2015	John Crossen		TFS# 1277 - Initial creation.
-----------------------------------------------------------------------------------------------------------------------





CREATE PROCEDURE [ECI].[usp_GetReasonForDelay]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		[ReasonForDelayID], [ReasonForDelay] 
		FROM		[ECI].[ReasonForDelay]
		WHERE		IsActive = 1
		ORDER BY	[ReasonForDelay] ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END


GO
