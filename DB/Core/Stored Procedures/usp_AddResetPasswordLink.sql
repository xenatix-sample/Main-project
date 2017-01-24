-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_AddResetPasswordLink
-- Author:		Rajiv Ranjan
-- Date:		08/24/2015
--
-- Purpose:		Add reset password link 
--
-- Notes:		N/A
--
-- Depends:		
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/24/2015 - Rajiv Ranjan	- Initial Creation
-- 08/26/2015 - Rajiv Ranjan	- Changed resetLink to resetIdentifier
-- 08/27/2015 - Rajiv Ranjan	- Update reset identifier if exists
-- 09/21/2015   John Crossen        Add Audit 
-- 09/25/2015   Arun Choudhary  - 2438 - Changed the audit Primary key to UserID from PhoneID
-- 09/28/2015   Justin Spalti	- Added the expiration date to the update statement
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added CreatedOn and CreatedBy fields
-- 03/06/2016   Satish Singh    - Commented usp_AddPreAuditLog and usp_AddPostAuditLog need to review by Scott
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_AddResetPasswordLink]
	@UserID INT,
	@ResetIdentifier UNIQUEIDENTIFIER,
	@RequestorIPAddress NVARCHAR(30),
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ResetPasswordID BIGINT,
		@NewID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
	IF EXISTS(SELECT 1 FROM Core.ResetPassword rp WHERE rp.UserID = @UserID AND rp.IsActive = 1)
		BEGIN
		SELECT @ResetPasswordID = ResetPasswordID FROM Core.ResetPassword WHERE UserID = @UserID AND IsActive = 1;

		--EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'ResetPassword', @ResetPasswordID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE [Core].ResetPassword
		SET	ResetIdentifier= @ResetIdentifier,
			RequestorIPAddress = @RequestorIPAddress,
			ExpiresOn = DATEADD(DAY,3, GETUTCDATE()), -- reset password link expires after 3 days
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		WHERE
			UserID = @UserID
			AND IsActive = 1;

		--EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'ResetPassword', @AuditDetailID, @ResetPasswordID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; 
		END
	ELSE
		BEGIN
		INSERT INTO Core.ResetPassword(
			UserID,
			ResetIdentifier,
			RequestorIPAddress,
			ExpiresOn,
			ModifiedOn,
			ModifiedBy,
			IsActive,
			CreatedBy,
			CreatedOn
		)
		VALUES(
			@UserID,
			@ResetIdentifier,
			@RequestorIPAddress,
			DATEADD(DAY,3, GETUTCDATE()), -- reset password link expires after 3 days
			@ModifiedOn,
			@ModifiedBy,
			1,
			@ModifiedBy,
			@ModifiedOn
		);

		SELECT @NewID = SCOPE_IDENTITY();

		--EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'ResetPassword', @NewID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
		--EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'ResetPassword', @AuditDetailID, @NewID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; 

		END
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
