
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_OpenEncryptionKeys]
-- Author:		Sumana Sangapu
-- Date:		04/01/2016
--
-- Purpose:		Code to open the Encryption Keys
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/01/2016	Sumana Sangapu  Initial creation.
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Core].[usp_OpenEncryptionKeys] 
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT

	AS
BEGIN
	DECLARE @AuditPost XML,
		@AuditID BIGINT;

	SELECT  @ResultCode = 0,
			@ResultMessage = 'Executed Successfully'

	BEGIN TRY

			OPEN SYMMETRIC KEY SymmetricXenatixKey DECRYPTION BY CERTIFICATE [EncryptionCertificate]	

	END TRY
	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH
END