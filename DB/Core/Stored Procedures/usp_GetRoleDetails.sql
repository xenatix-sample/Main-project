-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetRoleDetails]
-- Author:		Scott Martin
-- Date:		05/14/2016
--
-- Purpose:		Get Role details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 05/14/2016	Scott Martin		Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetRoleDetails]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
	SELECT 
		RoleID,
		Name,
		EffectiveDate,
		ExpirationDate,
		[Description]
	FROM 
		Core.[Role] R
	WHERE 
		IsActive = 1
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO