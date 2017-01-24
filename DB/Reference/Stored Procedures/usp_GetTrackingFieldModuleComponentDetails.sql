-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetTrackingFieldModuleComponentDetails]
-- Author:		Kyle Campbell
-- Date:		01/12/2017
--
-- Purpose:		Gets the list of TrackingFields and Service ModuleComponent mappings for Service Recording screens
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/04/2017	Kyle Campbell	TFS #14007	Initial Creation
-- 01/24/2017	Kyle Campbell	TFS #14007	Added IsActive in WHERE clause
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetTrackingFieldModuleComponentDetails]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT TFMC.TrackingFieldID, 
			TrackingField,
			TFMC.ServicesID,
			TFMC.ModuleComponentID,
			TFMC.IsActive,
			TFMC.ModifiedBy,
			TFMC.ModifiedOn,
			TFMC.IsActive  
		FROM Reference.TrackingFieldModuleComponent TFMC
			INNER JOIN Reference.[TrackingField] TF ON TFMC.TrackingFieldID = TF.TrackingFieldID
		WHERE TF.IsActive = 1
			AND TFMC.IsActive = 1
		ORDER BY TrackingField ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
