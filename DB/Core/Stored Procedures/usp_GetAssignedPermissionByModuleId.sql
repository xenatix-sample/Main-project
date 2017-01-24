-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetAssignedPermissionByModuleId]
-- Author:		Rajiv Ranjan
-- Date:		07/23/2015
--
-- Purpose:		Get assigned permission details by Module ID
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015	Rajiv Ranjan		Initial creation.
-- 08/03/2015 - John Crossen -- Change from dbo to Core schema
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetAssignedPermissionByModuleId]
	@ModuleID BIGINT,
	@RoleID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT 
			p.PermissionID,
			P.Name,
			p.Description ,
			p.Code,
			CAST(CASE WHEN mrp.ModuleRolePermissionID IS NULL THEN 0 ELSE 1 END AS BIT) AS Selected
		FROM 
			Core.Permission p 
			INNER JOIN Core.ModulePermission mp ON
			p.PermissionID=mp.PermissionID AND mp.ModuleID=@ModuleID
			LEFT JOIN Core.ModuleRolePermission mrp ON
			mp.ModuleID=mrp.ModuleID AND mrp.RoleID=@RoleID AND mp.PermissionID=mrp.PermissionID AND mrp.IsActive=1
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

GO