
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:   [usp_DeleteEntitySignatures]
-- Author:		Sumana Sangapu
-- Date:		03/22/2016
--
-- Purpose:		Delete EntitySignatures Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/22/2016	Sumana Sangapu	TFS:2700	Initial Creation 
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ESignature].[usp_DeleteEntitySignatures]
	@EntitySignatureID BIGINT,
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
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'ESignature', 'EntitySignatures', @EntitySignatureID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE	e
	SET	IsActive = 0,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	FROM
		[ESignature].[EntitySignatures] e
	WHERE
		EntitySignatureID = @EntitySignatureID;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'ESignature', 'EntitySignatures', @AuditDetailID, @EntitySignatureID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	
	END TRY

	BEGIN CATCH
	SELECT @ResultCode = ERROR_SEVERITY(),
			@ResultMessage = ERROR_MESSAGE()
	END CATCH
END