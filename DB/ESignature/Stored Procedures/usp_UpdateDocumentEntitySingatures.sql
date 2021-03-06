
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateDocumentEntitySingatures]
-- Author:		Sumana Sangapu
-- Date:		03/22/2016
--
-- Purpose:		Update Document Entity Signatures
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/22/2016	 Sumana Sangapu  Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ESignature].[usp_UpdateDocumentEntitySingatures]
	@DocumentEntitySignatureID BIGINT,
	@DocumentID BIGINT,
	@EntitySignatureID BIGINT,
	@DocumentTypeID INT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT 
AS

BEGIN
	DECLARE @AuditDetailID BIGINT,
			@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
			@EntityID BIGINT,
			@EntityTypeID INT;

  SELECT
    @ResultCode = 0,
    @ResultMessage = 'Executed Successfully'

  BEGIN TRY
	SELECT @EntityID = EntityID, @EntityTypeID = EntityTypeID FROM ESignature.EntitySignatures WHERE EntitySignatureID = @EntitySignatureID;

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'ESignature', 'DocumentEntitySignatures', @DocumentEntitySignatureID, NULL, NULL, NULL, @EntityID, @EntityTypeID, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

 		UPDATE	[ESignature].[DocumentEntitySignatures]  
		SET		
				DocumentID = @DocumentID,
				EntitySignatureID = @EntitySignatureID,
				DocumentTypeID = @DocumentTypeID,
				IsActive = 1, 
				ModifiedBy = @ModifiedBy, 
				ModifiedOn = @ModifiedOn, 
				SystemModifiedOn = GETUTCDATE() 
		WHERE	DocumentEntitySignatureID = @DocumentEntitySignatureID;
  
	EXEC Auditing.usp_AddPostAuditLog 'Update', 'ESignature', 'DocumentEntitySignatures', @AuditDetailID, @DocumentEntitySignatureID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
  
  END TRY

  BEGIN CATCH
    SELECT
      @ResultCode = ERROR_SEVERITY(),
      @ResultMessage = ERROR_MESSAGE()
  END CATCH

END
