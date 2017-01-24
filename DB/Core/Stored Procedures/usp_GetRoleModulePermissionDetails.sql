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

CREATE PROCEDURE [Core].[usp_GetRoleModulePermissionDetails]
	@RoleModuleID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
	SELECT
		RMP.RoleModulePermissionID,
		RMP.RoleModuleID,
		RMP.PermissionLevelID,
		PL.Name AS PermissionLevelName,
		RMP.PermissionID,
		P.Name AS PermissionName
	FROM 
		Core.RoleModulePermission RMP
		LEFT OUTER JOIN Core.Permission P
			ON RMP.PermissionID = P.PermissionID
		LEFT OUTER JOIN Core.PermissionLevel PL
			ON RMP.PermissionLevelID = PL.PermissionLevelID
	WHERE 
		RMP.IsActive = 1
		AND RMP.RoleModuleID = @RoleModuleID
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO