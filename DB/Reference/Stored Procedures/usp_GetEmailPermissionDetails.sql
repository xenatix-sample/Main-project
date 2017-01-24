-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetEmailPermissionDetails]
-- Author:		Rajiv Ranjan
-- Date:		08/15/2015
--
-- Purpose:		Gets the list of Email Permission lookup  details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/15/2015	Rajiv Ranjan		- Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetEmailPermissionDetails]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		
			[EmailPermissionID], 
			[EmailPermission] 
		FROM		
			[Reference].[EmailPermission] 
		WHERE
			IsActive = 1
		ORDER BY
			[EmailPermissionID]  ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

Go
