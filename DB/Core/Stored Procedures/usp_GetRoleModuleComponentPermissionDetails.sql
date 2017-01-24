-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetRoleModulePermissionDetails]
-- Author:		Scott Martin
-- Date:		05/14/2016
--
-- Purpose:		Get Role Module Permission details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 05/14/2016	Scott Martin		Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetRoleModuleComponentPermissionDetails]
	@RoleModuleID BIGINT,
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
		Core.RoleModuleComponentPermission RMP
		INNER JOIN Core.RoleModuleComponent RMC
			ON RMP.RoleModuleComponentID = RMC.RoleModuleComponentID
		LEFT OUTER JOIN Core.Permission P
			ON RMP.PermissionID = P.PermissionID
		LEFT OUTER JOIN Core.PermissionLevel PL
			ON RMP.PermissionLevelID = PL.PermissionLevelID
	WHERE 
		RMP.IsActive = 1
		AND RMC.RoleModuleID = @RoleModuleID
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO