-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetDeliveryMethodByServicesModuleComponentID]
-- Author:		Kyle Campbell
-- Date:		01/04/2017
--
-- Purpose:		Gets the list of Delivery Methods based on ServicesModuleComponentID
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/04/2017	Kyle Campbell	TFS #14007	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetDeliveryMethodsByServicesModuleComponentID]
	@ServicesID INT,
	@ModuleComponentID INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT DMMC.DeliveryMethodID, 
			DeliveryMethod,
			DMMC.IsActive,
			DMMC.ModifiedBy,
			DMMC.ModifiedOn  
		FROM Reference.DeliveryMethodModuleComponent DMMC 
			INNER JOIN Reference.DeliveryMethod DM ON DMMC.DeliveryMethodID = DM.DeliveryMethodID
		WHERE DMMC.ServicesID = @ServicesID AND DMMC.ModuleComponentID = @ModuleComponentID
			AND DMMC.IsActive = 1
			AND DM.IsActive = 1
		ORDER BY [DeliveryMethod] ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
