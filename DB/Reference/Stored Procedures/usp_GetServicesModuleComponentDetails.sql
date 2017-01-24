-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetServicesModuleComponentDetails]
-- Author:		Scott Martin
-- Date:		06/22/2016
--
-- Purpose:		Gets the list of Services Module Component lookup details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/21/2016	Scott Martin	- Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetServicesModuleComponentDetails]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
	SELECT
		SMC.ServicesModuleComponentID,
		S.ServicesID AS ServiceID,
		S.ServiceName,
		S.EffectiveDate,
		S.ExpirationDate,
		MC.ModuleComponentID,
		MC.DataKey
	FROM
		Reference.ServicesModuleComponent SMC
		INNER JOIN Reference.Services S
			ON SMC.ServicesID = S.ServicesID
		INNER JOIN Core.ModuleComponent MC
			ON SMC.ModuleComponentID = MC.ModuleComponentID
	WHERE
		SMC.IsActive = 1
		AND S.IsActive = 1
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END