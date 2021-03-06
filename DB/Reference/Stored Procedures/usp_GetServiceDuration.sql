-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetServiceDuration]
-- Author:		Rajiv Ranjan
-- Date:		12/16/2016
--
-- Purpose:		Gets the list of County details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/16/2016	Rajiv Ranjan		Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetServiceDuration]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		[ServiceDurationID], [ServiceDurationStart], [ServiceDurationEnd],[ServiceDurationDisplay],[SortOrder]
		FROM		[Reference].[ServiceDuration] 
		WHERE		IsActive = 1
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END