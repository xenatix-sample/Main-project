-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_UpdateUserSignatureDetails
-- Author:		Sumana Sangapu
-- Date:		03/24/2016
--
-- Purpose:		Save User DigitalPassword and Signature
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/24/2016 - Sumana Sangapu Initial Creation 
-- 04/11/2016	Sumana Sangapu Modified to store the Digital Password as DB encrypted.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_UpdateUserSignatureDetails]
	@UserID INT,
	@UpdatePassword BIT,
	@NewPassword NVARCHAR(100),
	@CurrentPassword NVARCHAR(100),
	@PrintSignature nvarchar(100),
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'Executed successfully'

	DECLARE @ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
			@EncryptedValue varbinary(2000)
			
	DECLARE @AuditDetailID BIGINT;

	-- Add audit calls 

	BEGIN TRY

		EXEC Core.usp_OpenEncryptionkeys @ResultCode OUTPUT, @ResultMessage OUTPUT

		SELECT @EncryptedValue = Core.fn_Encrypt(@NewPassword)

		IF @UpdatePassword = 1
		BEGIN
			IF NOT EXISTS(
					  SELECT 1 
					  FROM Core.Users u 
					  WHERE u.UserID = @UserID 
					 )
			BEGIN
				RAISERROR('The current password could not be verified.', 16, 1)
			END
		END
	
		UPDATE Core.Users
		SET [DigitalPassword] = CASE WHEN @NewPassword IS NOT NULL THEN @EncryptedValue ELSE [DigitalPassword] END,
			PrintSignature = @PrintSignature,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		WHERE UserID = @UserID


	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END