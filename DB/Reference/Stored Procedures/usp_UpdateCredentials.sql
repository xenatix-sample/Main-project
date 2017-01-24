-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	Reference.[usp_UpdateCredentials]
-- Author:		John Crossen
-- Date:		08/12/2015
--
-- Purpose:		Update Credential details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/12/2015	John Crossen TFS#1182 		Initial creation.
-- 9/23/2015    John Crossen                Add Audit
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added SystemModifiedOn field
-- 04/06/2016	Sumana Sangapu		Removed CredentialCode 
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Reference.[usp_UpdateCredentials]
	@CredentialID BIGINT,
	@CredentialAbbreviation NVARCHAR(20),
	@CredentialName NVARCHAR(255),
	@EffectiveDate DATE,
	@ExpirationDate DATE NULL,
	@ExpirationReason NVARCHAR(500),
	@LicenseRequired BIT,
	@IsInternal BIT,
	@IsActive BIT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Reference', 'Credentials', @CredentialID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;	

	UPDATE [Reference].[Credentials]
	SET [CredentialAbbreviation] = @CredentialAbbreviation,
		[CredentialName] = @CredentialName,
		[EffectiveDate] = @EffectiveDate,
		[ExpirationDate] = @ExpirationDate,
		[ExpirationReason] = @ExpirationReason,
		[LicenseRequired] =@LicenseRequired,
		[IsInternal] = @IsInternal,
		[IsActive] = @IsActive,
		[ModifiedBy] = @ModifiedBy,
		[ModifiedOn] = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	 WHERE
		CredentialID = @CredentialID;
		
	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Reference', 'Credentials', @AuditDetailID, @CredentialID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END


