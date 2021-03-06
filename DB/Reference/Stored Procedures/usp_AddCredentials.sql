
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	Reference.[usp_AddCredentials]
-- Author:		John Crossen
-- Date:		08/12/2015
--
-- Purpose:		Add Credential details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/12/2015	John Crossen TFS#1182 		Initial creation.
-- 9/23/2015    John Crossen                Add Audit
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added CreatedBy and CreatedOn fields
-- 04/06/2016	Sumana Sangapu		Removed CredentialCode 
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Reference.[usp_AddCredentials]
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
	IF EXISTS (SELECT 1 FROM Reference.[Credentials] WHERE [CredentialName] = @CredentialName AND [CredentialAbbreviation] = @CredentialAbbreviation AND [IsActive] = 1)
		 BEGIN
			RAISERROR ('Credential name can not be duplicate.', -- Message text.
               16, -- Severity.
               1 -- State.
               );
		 END

	DECLARE @ID BIGINT;
	INSERT INTO [Reference].[Credentials]
    (
		[CredentialAbbreviation],
		[CredentialName],
		[EffectiveDate],
		[ExpirationDate],
		[ExpirationReason],
		[LicenseRequired],
		[IsInternal],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn],
		CreatedBy,
		CreatedOn
	)
	VALUES
	(
		@CredentialAbbreviation,
		@CredentialName,
		@EffectiveDate,
		@ExpirationDate,
		@ExpirationReason,
		@LicenseRequired,
		@IsInternal,
		@IsActive,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	)

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Reference', 'Credentials', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Reference', 'Credentials', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END