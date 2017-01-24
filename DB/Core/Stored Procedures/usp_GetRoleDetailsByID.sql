-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetRoleDetailsByID]
-- Author:		Scott Martin
-- Date:		05/14/2016
--
-- Purpose:		Get details for specific role
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 05/14/2016	Scott Martin		Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetRoleDetailsByID]
	@RoleID BIGINT,
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
		AND RoleID = @RoleID
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO