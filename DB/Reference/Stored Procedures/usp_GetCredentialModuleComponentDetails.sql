-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetCredentialModuleComponentDetails]
-- Author:		Scott Martin
-- Date:		08/10/2016
--
-- Purpose:		Gets the list of Credential Module Component lookup details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/10/2016	Scott Martin	- Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetCredentialModuleComponentDetails]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
	SELECT
		CMC.CredentialModuleComponentID,
		CR.CredentialID,
		CR.CredentialAbbreviation,
		CMC.ServicesID AS ServiceID,
		MC.ModuleComponentID,
		MC.DataKey
	FROM
		Reference.CredentialModuleComponent CMC
		INNER JOIN Reference.[Credentials] CR
			ON CMC.CredentialID = CR.CredentialID
		INNER JOIN Core.ModuleComponent MC
			ON CMC.ModuleComponentID = MC.ModuleComponentID
	WHERE
		CMC.IsActive = 1
		AND CR.IsActive = 1
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END