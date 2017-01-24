-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetServicesModuleMappingDetails]
-- Author:		Scott Martin
-- Date:		06/21/2016
--
-- Purpose:		Gets the list of Services Module Mapping lookup details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/21/2016	Scott Martin	- Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetServicesModuleMappingDetails]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
	SELECT
		ServicesModuleMappingID,
		ServicesID AS ServiceID,
		ModuleID
	FROM
		Reference.ServicesModuleMapping SMM
	WHERE
		SMM.IsActive = 1
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END