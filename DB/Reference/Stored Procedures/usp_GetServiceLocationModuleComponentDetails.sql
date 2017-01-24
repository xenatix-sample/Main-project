-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetServiceLocationModuleComponentDetails]
-- Author:		Scott Martin
-- Date:		06/22/2016
--
-- Purpose:		Gets the list of Service Location Module Component lookup details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/22/2016	Scott Martin	- Initial creation.
-- 01/24/2017	Kyle Campbell	TFS #14007	Added IsActive in WHERE clause
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetServiceLocationModuleComponentDetails]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
	SELECT
		SLMC.ServiceLocationModuleComponentID,
		SL.ServiceLocationID,
		SL.ServiceLocation,
		SLMC.ServicesID AS ServiceID,
		SLMC.IsActive,
		MC.ModuleComponentID,
		MC.DataKey
	FROM
		Reference.ServiceLocationModuleComponent SLMC
		INNER JOIN Reference.ServiceLocation SL
			ON SLMC.ServiceLocationID = SL.ServiceLocationID
		INNER JOIN Core.ModuleComponent MC
			ON SLMC.ModuleComponentID = MC.ModuleComponentID
	WHERE SL.IsActive = 1
		AND SLMC.IsActive = 1
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END