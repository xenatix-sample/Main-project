
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetContactSSN]
-- Author:		Arun Choudhary
-- Date:		03/31/2016
--
-- Purpose:		Get Contact Full SSN
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_GetContactSSN]
	@ContactID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'
	BEGIN TRY
		EXEC Core.usp_OpenEncryptionKeys @ResultCode OUTPUT, @ResultMessage OUTPUT
		SELECT 
			CONVERT(NVARCHAR(9), Core.fn_Decrypt(c.SSNEncrypted)) SSN
		FROM 
			Registration.Contact c
		WHERE 
			c.ContactID = @ContactID AND c.IsActive = 1
	END TRY
	BEGIN CATCH
		SELECT 
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END

