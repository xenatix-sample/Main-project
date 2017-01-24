
----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_GetCredentialAction]
-- Author:		Sumana Sangapu
-- Date:		04/09/2016
--
-- Purpose:		Gets the lookup values for CredentialAction
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/09/2016	Sumana Sangapu    Initial creation. 
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetCredentialAction]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		CredentialActionID, CredentialAction
		FROM		[Reference].[CredentialAction] 
		WHERE		IsActive = 1
		ORDER BY	CredentialAction ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END