-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetModulePermissions]
-- Author:		Rajiv Ranjan
-- Date:		07/23/2015
--
-- Purpose:		Get Permissions details of Module
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015	Rajiv Ranjan		Initial creation.
-- 08/03/2015 - John Crossen -- Change from dbo to Core schema
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetModulePermissions]
	@RoleName NVARCHAR(100),
	@ModuleName NVARCHAR(100),
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT 
			mp.ModulePermissionID, 
			mp.ModuleID, 
			mp.PermissionID, 
		    mp.ModifiedOn, 
			mp.ModifiedBy
		FROM 
			Core.ModulePermission mp
			JOIN Core.RoleModule rm
				ON rm.ModuleID = mp.ModuleID
			JOIN Core.Module m
				ON m.ModuleID = rm.ModuleID
			JOIN Core.[Role] r
				ON r.RoleID = rm.RoleID
			WHERE r.Name = @RoleName
				AND m.Name = @ModuleName
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
