-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetRoleModuleDetails]
-- Author:		Scott Martin
-- Date:		05/14/2016
--
-- Purpose:		Get Role Module details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 05/14/2016	Scott Martin		Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetRoleModuleDetails]
	@RoleModuleID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
	SELECT 
		RM.RoleModuleID,
		M.Name,
		M.[Description],
		RM.PermissionLevelID,
		R.Name AS RoleName 
	FROM 
		Core.[RoleModule] RM
		INNER JOIN Core.Module M
			ON RM.ModuleID = M.ModuleID
		LEFT JOIN Core.Role R
			ON RM.RoleID = R.RoleID
	WHERE 
		RM.IsActive = 1
		AND RM.RoleModuleID = @RoleModuleID
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO