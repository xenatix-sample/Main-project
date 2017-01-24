-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetModuleComponentsForServiceMapping]
-- Author:		Kyle Campbell
-- Date:		01/04/2017
--
-- Purpose:		Gets the list of Module Components that allow service mappings
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/04/2017	Kyle Campbell	TFS #14007	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetModuleComponentsForServiceMapping]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT 
			F.Name As Feature, 
			ModuleComponentID,
			MC.IsActive,
			MC.ModifiedBy,
			MC.ModifiedOn
		FROM 
			Core.ModuleComponent MC 
			INNER JOIN Core.Feature F ON MC.FeatureID = F.FeatureID
		WHERE 
			AllowServiceMapping = 1
			AND MC.IsActive = 1
		ORDER BY F.Name ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
