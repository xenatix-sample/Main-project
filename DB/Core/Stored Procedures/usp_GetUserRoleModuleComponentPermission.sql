-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetUserRoleModuleComponentPermission]
-- Author:		Rajiv Ranjan
-- Date:		05/20/2016
--
-- Purpose:		Get Role Module Permission details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 05/20/2016	Rajiv Ranjan		Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetUserRoleModuleComponentPermission]
	@UserID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
	SELECT
		RMP.RoleModuleComponentPermissionID,
		RMP.RoleModuleComponentID,
		RMP.PermissionLevelID,
		PL.Name AS PermissionLevelName,
		RMP.PermissionID,
		P.Name AS PermissionName
	FROM 
		Core.UserRole UR
		INNER JOIN Core.Role R
			ON UR.RoleID = R.RoleID
		INNER JOIN Core.RoleModule RM
			ON R.RoleID = RM.RoleID		
		INNER JOIN Core.RoleModuleComponent RMC
			ON RM.RoleModuleID = RMC.RoleModuleID			
		INNER JOIN Core.RoleModuleComponentPermission RMP
			ON RMC.RoleModuleComponentID = RMP.RoleModuleComponentID		
		LEFT OUTER JOIN Core.Permission P
			ON RMP.PermissionID = P.PermissionID
		LEFT OUTER JOIN Core.PermissionLevel PL
			ON RMP.PermissionLevelID = PL.PermissionLevelID
	WHERE 
		UR.IsActive=1
		AND R.IsActive=1
		AND RM.IsActive=1
		AND RMC.IsActive = 1
		AND RMP.IsActive=1
		AND P.IsActive=1
		AND RMP.PermissionLevelID IS NOT NULL
		AND UR.UserID = @UserID
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO