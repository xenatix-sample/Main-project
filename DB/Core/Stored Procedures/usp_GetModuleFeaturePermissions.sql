-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetModuleFeaturePermissions]
-- Author:		Rajiv Ranjan
-- Date:		07/28/2015
--
-- Purpose:		Get user's role permission
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/28/2015	Rajiv Ranjan		Initial creation.
-- 07/30/2015   John Crossen     Change schema from dbo to Core		
-- 08/03/2015 - John Crossen -- Change from dbo to Core schema
-- 08/28/2015 - Rajiv Ranjan	Added IsActive check
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetModuleFeaturePermissions]
	@UserID INT,
	@ModuleID INT,
	@FeatureID INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT   
			mrp.PermissionID,
			p.Name,
			p.Description,
			p.Code
		FROM  
			Core.UserRole ur 
			INNER JOIN Core.Role r ON r.RoleID = ur.RoleID AND ur.UserID = @UserID
			INNER JOIN Core.ModuleRolePermission mrp ON mrp.RoleID = r.RoleID AND mrp.ModuleID = @ModuleID
			INNER JOIN Core.Module m ON m.ModuleID = mrp.ModuleID 
			INNER JOIN Core.Permission p ON mrp.PermissionID = p.PermissionID
		WHERE
			ur.IsActive=1 AND
			r.IsActive=1 AND
			mrp.IsActive=1 AND
			m.IsActive=1 AND
			p.IsActive=1 

		UNION

		SELECT   
			frp.PermissionID,
			p.Name,
			p.Description,
			p.Code
		FROM  
			Core.UserRole ur 
			INNER JOIN Core.Role r ON r.RoleID = ur.RoleID AND ur.UserID = @UserID
			INNER JOIN Core.FeatureRolePermission frp ON frp.RoleID = r.RoleID AND frp.FeatureID = @FeatureID
			INNER JOIN Core.Feature f ON f.FeatureID = frp.FeatureID
			INNER JOIN Core.Permission p ON frp.PermissionID = p.PermissionID
		WHERE
			ur.IsActive=1 AND
			r.IsActive=1 AND
			frp.IsActive=1 AND
			f.IsActive=1 AND
			p.IsActive=1 

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

GO