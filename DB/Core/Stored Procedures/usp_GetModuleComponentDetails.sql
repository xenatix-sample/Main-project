-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetModuleComponentDetails]
-- Author:		Scott Martin
-- Date:		05/14/2016
--
-- Purpose:		Get Module Component details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 05/14/2016	Scott Martin		Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetModuleComponentDetails]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
	SELECT 
		MC.ModuleComponentID,
		MC.ModuleID,
		M.Name AS ModuleName,
		MC.FeatureID,
		F.Name AS FeatureName,
		MC.ComponentID,
		C.Name AS ComponentName,
		C.ComponentTypeID,
		CT.ComponentType,
		MC.DataKey
	FROM
		Core.ModuleComponent MC
		INNER JOIN Core.Module M
			ON MC.ModuleID = M.ModuleID
		LEFT OUTER JOIN Core.Feature F
			ON MC.FeatureID = F.FeatureID
		INNER JOIN Core.[Component] C
			ON MC.ComponentID = C.ComponentID
		INNER JOIN Core.ComponentType CT
			ON C.ComponentTypeID = CT.ComponentTypeID
	WHERE 
		C.IsActive = 1
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO