-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetTrackingFieldsByServicesModuleComponentID]
-- Author:		Kyle Campbell
-- Date:		01/04/2017
--
-- Purpose:		Gets the list of TrackingFields based on ServicesModuleComponentID
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/04/2017	Kyle Campbell	TFS #14007	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetTrackingFieldsByServicesModuleComponentID]
	@ServicesID INT,
	@ModuleComponentID INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT TFMC.TrackingFieldID, 
			TrackingField,
			TFMC.IsActive,
			TFMC.ModifiedBy,
			TFMC.ModifiedOn  
		FROM Reference.TrackingFieldModuleComponent TFMC
			INNER JOIN Reference.[TrackingField] TF ON TFMC.TrackingFieldID = TF.TrackingFieldID
		WHERE TFMC.ServicesID = @ServicesID AND TFMC.ModuleComponentID = @ModuleComponentID			
			AND TFMC.IsActive = 1
			AND TF.IsActive = 1
		ORDER BY TrackingField ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
