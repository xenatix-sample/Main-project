-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetServiceStatusModuleComponentDetails]
-- Author:		Kyle Campbell
-- Date:		01/012/2017
--
-- Purpose:		Gets the list of ServiceStatus based on ServicesModuleComponentID
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/12/2017	Kyle Campbell	TFS #14007	Initial Creation
-- 01/24/2017	Kyle Campbell	TFS #14007	Added IsActive in WHERE clause
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetServiceStatusModuleComponentDetails]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT SSMC.ServiceStatusID, 
			ServiceStatus,
			SSMC.ServicesID,
			SSMC.ModuleComponentID,
			SSMC.IsActive,
			SSMC.ModifiedBy,
			SSMC.ModifiedOn,
			SSMC.IsActive  
		FROM Reference.ServiceStatusModuleComponent SSMC
			INNER JOIN Reference.[ServiceStatus] SS ON SSMC.ServiceStatusID = SS.ServiceStatusID
		WHERE SS.IsActive = 1
			AND SSMC.IsActive = 1
		ORDER BY ServiceStatus ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
