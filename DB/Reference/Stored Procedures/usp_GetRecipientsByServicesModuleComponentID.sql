----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetRecipientsByServicesModuleComponentID]
-- Author:		Kyle Campbell
-- Date:		01/04/2017
--
-- Purpose:		Gets the list of Recipients based on ServicesModuleComponentID
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/04/2017	Kyle Campbell	TFS #14007	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetRecipientsByServicesModuleComponentID]
	@ServicesID INT,
	@ModuleComponentID INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT RCMC.RecipientCodeID as CodeID, 
			RC.CodeDescription,
			RCMC.IsActive,
			RCMC.ModifiedBy,
			RCMC.ModifiedOn  
		FROM Reference.RecipientCodeModuleComponent RCMC 
			INNER JOIN Reference.RecipientCode RC ON RCMC.RecipientCodeID = RC.CodeID
		WHERE RCMC.ServicesID = @ServicesID AND RCMC.ModuleComponentID = @ModuleComponentID		
			AND RCMC.IsActive = 1
		ORDER BY CodeDescription ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

	