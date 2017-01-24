-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_GetOrganizationDetailsModuleComponentByDetailID]
-- Author:		Scott Martin
-- Date:		01/15/2017
--
-- Purpose:		Get Organization Details Module Component details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/15/2017	Scott Martin	Initial creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Core.usp_GetOrganizationDetailsModuleComponentByDetailID
	@DetailID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	BEGIN TRY
		SELECT @ResultCode = 0,
			   @ResultMessage = 'executed successfully'
		
		SELECT
			ODMC.OrganizationDetailsModuleComponentID,
			ODMC.DetailID,
			ODMC.ModuleComponentID,
			F.Name AS Feature
		FROM
			Core.OrganizationDetailsModuleComponent ODMC
			INNER JOIN Core.ModuleComponent MC
				ON ODMC.ModuleComponentID = MC.ModuleComponentID
			INNER JOIN Core.Feature F
				ON MC.FeatureID = F.FeatureID
		WHERE
			ODMC.DetailID = @DetailID
			AND ODMC.IsActive = 1;
  	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

GO