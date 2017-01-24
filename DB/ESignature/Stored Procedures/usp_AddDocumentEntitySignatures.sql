
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddDocumentEntitySignatures]
-- Author:		Sumana Sangapu
-- Date:		03/22/2016
--
-- Purpose:		MAp the  DocumentID and EntitySignatureID. DocumentID is the PK of any of the DocumentTypes. IT could be AssessmentID, ScreeningID, consentID, etc.
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/22/2016	Sumana Sangapu - Initial creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ESignature].[usp_AddDocumentEntitySignatures]
	@DocumentID BIGINT,
	@EntitySignatureID BIGINT,
	@DocumentTypeID int,
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
	DECLARE @ID BIGINT

	-- Insert into ESignature.DocumentEntitySignatures	
	INSERT INTO ESignature.DocumentEntitySignatures
	(
		DocumentID,
		EntitySignatureID,
		DocumentTypeID,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	VALUES
	(	
		@DocumentID,	
		@EntitySignatureID,
		@DocumentTypeID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	)

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'ESignature', 'DocumentEntitySignatures', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'ESignature', 'DocumentEntitySignatures', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE();
	END CATCH
END