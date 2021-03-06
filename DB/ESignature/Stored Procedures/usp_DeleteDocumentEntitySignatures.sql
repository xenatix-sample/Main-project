
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:   [usp_DeleteDocumentEntitySignatures]
-- Author:		Sumana Sangapu
-- Date:		03/22/2016
--
-- Purpose:		Delete DocumentEntitySignatures Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/22/2016	Sumana Sangapu	Initial Creation 
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ESignature].[usp_DeleteDocumentEntitySignatures]
	@DocumentEntitySignatureID BIGINT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

SELECT
    @ResultCode = 0,
    @ResultMessage = 'Executed Successfully'

	BEGIN TRY
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'ESignature', 'DocumentEntitySignatures', @DocumentEntitySignatureID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE	e
	SET	IsActive = 0,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	FROM
		[ESignature].[DocumentEntitySignatures] e
	WHERE
		DocumentEntitySignatureID = @DocumentEntitySignatureID;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'ESignature', 'DocumentEntitySignatures', @AuditDetailID, @DocumentEntitySignatureID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	
	END TRY

	BEGIN CATCH
	SELECT @ResultCode = ERROR_SEVERITY(),
			@ResultMessage = ERROR_MESSAGE()
	END CATCH
END