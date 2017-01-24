-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetUserRoleModuleComponentSecurity]
-- Author:		Scott Martin
-- Date:		05/17/2016
--
-- Purpose:		Get user's assigned roles and module components
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 05/17/2016	Scott Martin		Initial creation.
-- 08/25/2016	Gurpreet Singh		Added validation for Effective and Expiration Date
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetUserRoleModuleComponentSecurity]
	@UserID INT,	
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
	
	DECLARE @minDate DATETIME
	SELECT @minDate = cast(-53690 AS DATETIME)

	SELECT DISTINCT
		R.RoleID,
		R.Name AS RoleName,
		M.ModuleID,
		M.Name AS ModuleName,
		RM.RoleModuleID,		
		MC.DataKey,
		C.ComponentID,
		C.Name AS ComponentName,
		MC.ModuleComponentID,
		RMC.RoleModuleComponentID
	FROM
		Core.UserRole UR
		INNER JOIN Core.Role R
			ON UR.RoleId = R.RoleID
		INNER JOIN Core.RoleModule RM
			ON R.RoleID = RM.RoleID
		INNER JOIN Core.Module M
			ON RM.ModuleID = M.ModuleID
		INNER JOIN Core.RoleModuleComponent RMC
			ON RM.RoleModuleID = RMC.RoleModuleID
		INNER JOIN Core.ModuleComponent MC
			ON RMC.ModuleComponentID = MC.ModuleComponentID
		INNER JOIN Core.Component C
			ON MC.ComponentID = C.ComponentID
	WHERE
		UR.UserID = @UserID
		AND UR.IsActive =1 
		AND RM.IsActive=1
		AND RMC.IsActive=1
		AND MC.IsActive=1
		AND C.IsActive=1
		AND (ISNULL(R.EffectiveDate,@minDate) = @minDate OR  R.EffectiveDate <= GETDATE())
		AND (ISNULL(R.ExpirationDate,@minDate) = @minDate OR  R.ExpirationDate >= GETDATE())
	ORDER BY
		M.ModuleID,
		MC.ModuleComponentID
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

GO