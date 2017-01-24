-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetCredentialsByServicesModuleComponentID]
-- Author:		Kyle Campbell
-- Date:		01/04/2017
--
-- Purpose:		Gets the list of Credentials based on ServicesModuleComponentID
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/04/2017	Kyle Campbell	TFS #14007	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetCredentialsByServicesModuleComponentID]
	@ServicesID INT,
	@ModuleComponentID INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT CMC.CredentialID, 
			CredentialName,
			CMC.IsActive,
			CMC.ModifiedBy,
			CMC.ModifiedOn  
		FROM Reference.CredentialModuleComponent CMC
			INNER JOIN Reference.[Credentials] C ON CMC.CredentialID = C.CredentialID
		WHERE CMC.ServicesID = @ServicesID AND CMC.ModuleComponentID = @ModuleComponentID			
			AND CMC.IsActive = 1
			AND C.IsActive = 1
		ORDER BY CredentialName ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
