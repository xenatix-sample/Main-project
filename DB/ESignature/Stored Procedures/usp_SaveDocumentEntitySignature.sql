-- Procedure:	[usp_AddDocumentEntitySignature]
-- Author:		Demetrios C. Christopher
-- Date:		11/02/2015
--
-- Purpose:		Add a signature for a specific person to a specific document
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/02/2015	Demetrios C. Christopher - Initial Creation
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added CreatedBy and CreatedOn fields
-- 05/23/2016	Gurmant Singh	Added The EntityName parameter and update the EntityName value based on the EntityTypeID
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ESignature].[usp_SaveDocumentEntitySignature]
	@DocumentID BIGINT,
	@DocumentTypeID INT,
	@EntitySignatureID BIGINT, -- May need later for signature re-use
	@EntityID BIGINT,
	@EntityName NVARCHAR(100),
	@EntityTypeID INT,
	@SignatureID BIGINT,
	@CredentialID BIGINT,
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
		   @ResultMessage = 'executed successfully';

	BEGIN TRY	
	DECLARE @ID BIGINT;

	--Get the Entity name from the EntityID based on the EntityTypeID if exists and then insert the value
	IF(@EntityID > 0 AND (@EntityName IS NULL))
	BEGIN
		Select @EntityName = Case @EntityTypeID
			WHEN 1
				THEN (Select FirstName + ' ' + LastName From Core.Users
						WHERE UserID = @EntityID)
			WHEN 2
				THEN (Select FirstName + ' ' + LastName From Registration.Contact
						WHERE ContactID = @EntityID) 
		END
	END

	IF (ISNULL(@EntitySignatureID, 0) = 0)
		BEGIN
		INSERT INTO ESignature.EntitySignatures
		(
			EntityID,
			EntityName,
			EntityTypeID,
			SignatureID,
			CredentialID,
			ModifiedOn,
			ModifiedBy,
			IsActive,
			CreatedBy,
			CreatedOn
		)
		VALUES
		(
			@EntityID,
			@EntityName,
			@EntityTypeID,
			@SignatureID,
			@CredentialID,
			@ModifiedOn,
			@ModifiedBy,
			@IsActive,
			@ModifiedBy,
			@ModifiedOn
		);

		SELECT @EntitySignatureID = SCOPE_IDENTITY();

		EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'ESignature', 'EntitySignatures', @EntitySignatureID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'ESignature', 'EntitySignatures', @AuditDetailID, @EntitySignatureID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		END

	INSERT INTO ESignature.DocumentEntitySignatures
	(
		DocumentID,
		DocumentTypeID,
		EntitySignatureID,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	VALUES
	(
		@DocumentID,
		@DocumentTypeID,
		@EntitySignatureID,
		@IsActive,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'ESignature', 'DocumentEntitySignatures', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'ESignature', 'DocumentEntitySignatures', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END