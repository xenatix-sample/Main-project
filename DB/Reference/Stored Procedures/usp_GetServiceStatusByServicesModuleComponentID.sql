-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetServiceStatusByServicesModuleComponentID]
-- Author:		Kyle Campbell
-- Date:		01/04/2017
--
-- Purpose:		Gets the list of ServiceStatus based on ServicesModuleComponentID
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/04/2017	Kyle Campbell	TFS #14007	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetServiceStatusByServicesModuleComponentID]
	@ServicesID INT,
	@ModuleComponentID INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT SSMC.ServiceStatusID, 
			ServiceStatus,
			SSMC.IsActive,
			SSMC.ModifiedBy,
			SSMC.ModifiedOn  
		FROM Reference.ServiceStatusModuleComponent SSMC
			INNER JOIN Reference.[ServiceStatus] SS ON SSMC.ServiceStatusID = SS.ServiceStatusID
		WHERE SSMC.ServicesID = @ServicesID AND SSMC.ModuleComponentID = @ModuleComponentID			
			AND SSMC.IsActive = 1
			AND SS.IsActive = 1
		ORDER BY ServiceStatus ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
