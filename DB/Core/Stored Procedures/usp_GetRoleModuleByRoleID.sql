-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetRoleModuleByRoleID]
-- Author:		Scott Martin
-- Date:		05/14/2016
--
-- Purpose:		Get Role Module for a role
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 05/14/2016	Scott Martin		Initial creation.
-- 05/27/2016	Scott Martin		Moved RoleModule IsActive criteria to the join 
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetRoleModuleByRoleID]
	@RoleID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
	SELECT
		M.ModuleID,
		RM.RoleID,
		RM.RoleModuleID,
		M.Name,
		M.[Description],
		RM.PermissionLevelID
	FROM
		Core.Module M
		LEFT OUTER JOIN Core.[RoleModule] RM
			ON M.ModuleID = RM.ModuleID
			AND RM.RoleID = @RoleID
			AND ISNULL(RM.IsActive, 1) = 1
	WHERE 
		M.IsActive = 1
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO