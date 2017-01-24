-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetDeliveryMethodModuleComponentDetails]
-- Author:		Scott Martin
-- Date:		06/22/2016
--
-- Purpose:		Gets the list of Delivery Method Module Component lookup details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/22/2016	Scott Martin	- Initial creation.
-- 01/24/2017	Kyle Campbell	TFS #14007	Added IsActive in WHERE clause
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetDeliveryMethodModuleComponentDetails]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
	SELECT
		DMMC.DeliveryMethodModuleComponentID,
		DM.DeliveryMethodID,
		DM.DeliveryMethod,
		DMMC.ServicesID AS ServiceID,
		MC.ModuleComponentID,
		MC.DataKey,
		DMMC.IsActive
	FROM
		Reference.DeliveryMethodModuleComponent DMMC
		INNER JOIN Reference.DeliveryMethod DM
			ON DMMC.DeliveryMethodID = DM.DeliveryMethodID
		INNER JOIN Core.ModuleComponent MC
			ON DMMC.ModuleComponentID = MC.ModuleComponentID
	WHERE DM.IsActive = 1
		AND DMMC.IsActive = 1
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END