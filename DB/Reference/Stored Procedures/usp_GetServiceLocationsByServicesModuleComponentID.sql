-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetServiceLocationByServicesModuleComponentID]
-- Author:		Kyle Campbell
-- Date:		01/04/2017
--
-- Purpose:		Gets the list of Service Locations based on ServicesModuleComponentID
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/04/2017	Kyle Campbell	TFS #14007	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetServiceLocationsByServicesModuleComponentID]
	@ServicesID INT,
	@ModuleComponentID INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT SLMC.ServiceLocationID, 
			ServiceLocation,
			SLMC.IsActive,
			SLMC.ModifiedBy,
			SLMC.ModifiedOn  
		FROM Reference.ServiceLocationModuleComponent SLMC 
			INNER JOIN Reference.ServiceLocation SL ON SLMC.ServiceLocationID = SL.ServiceLocationID
		WHERE SLMC.ServicesID = @ServicesID AND SLMC.ModuleComponentID = @ModuleComponentID			
			AND SLMC.IsActive = 1
			AND SL.IsACtive = 1
		ORDER BY ServiceLocation ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
