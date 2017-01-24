-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetRoleModuleComponentDetails]
-- Author:		Scott Martin
-- Date:		05/14/2016
--
-- Purpose:		Get Role Module Component details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 05/14/2016	Scott Martin		Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetRoleModuleComponentDetails]
	@RoleModuleID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
	SELECT
		RMC.RoleModuleComponentID,
		RMC.RoleModuleID,
		MC.ModuleComponentID,
		MC.ModuleID,
		M.Name AS ModuleName,
		MC.FeatureID,
		F.Name AS FeatureName,
		MC.ComponentID,
		C.Name AS ComponentName,
		C.ComponentTypeID,
		CT.ComponentType,
		MC.DataKey,
		RMC.PermissionLevelID
	FROM
		Core.ModuleComponent MC
		INNER JOIN Core.RoleModule RM
			ON MC.ModuleID = RM.ModuleID
		LEFT OUTER JOIN Core.RoleModuleComponent RMC
			ON MC.ModuleComponentID = RMC.ModuleComponentID
			AND RMC.RoleModuleID = @RoleModuleID
		INNER JOIN Core.Module M
			ON MC.ModuleID = M.ModuleID
		LEFT OUTER JOIN Core.Feature F
			ON MC.FeatureID = F.FeatureID
		INNER JOIN Core.[Component] C
			ON MC.ComponentID = C.ComponentID
		INNER JOIN Core.ComponentType CT
			ON C.ComponentTypeID = CT.ComponentTypeID
	WHERE 
		ISNULL(RMC.IsActive, 1) = 1
		AND MC.IsActive = 1
		AND RM.RoleModuleID = @RoleModuleID
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO