-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetContactConsents]
-- Author:		Kyle Campbell
-- Date:		03/15/2016
--
-- Purpose:		Gets the list of Consents for Contact
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/16/2016	Kyle Campbell	TFS #7829 Initial Creation
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Core].[usp_GetContactConsents]
	@ContactID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS

BEGIN
	SELECT	@ResultCode = 0,
			@ResultMessage = 'executed successfully';

	BEGIN TRY
		SELECT ContactConsentID, ContactID, ResponseID, DateSigned, ExpirationDate, SignatureStatusID
		FROM Core.ContactConsents
		WHERE ContactID = @ContactID AND IsActive = 1
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
