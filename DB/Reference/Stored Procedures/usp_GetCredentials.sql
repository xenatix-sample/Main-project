-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	Reference.[usp_GetCredentials]
-- Author:		John Crossen
-- Date:		08/12/2015
--
-- Purpose:		Select Credential details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/12/2015	John Crossen TFS#1182 		Initial creation.
-- 06/07/2016	Kyle Campbell	TFS #11310	Return non-IsSystem values
-------------------------------------------------------------------------------------------------------------------------



CREATE PROCEDURE [Reference].[usp_GetCredentials]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	


SELECT [CredentialID]
      ,[CredentialAbbreviation]
      ,[CredentialName]
      ,[EffectiveDate]
      ,[ExpirationDate]
      ,[ExpirationReason]
      ,[IsInternal]
      ,[IsActive]
      ,[ModifiedBy]
      ,[ModifiedOn]
  FROM [Reference].[Credentials]
  WHERE [IsSystem] <> 1
  ORDER BY [CredentialName] ASC 

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


