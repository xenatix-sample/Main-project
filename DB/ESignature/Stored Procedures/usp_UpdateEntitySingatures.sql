
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateEntitySingatures]
-- Author:		Sumana Sangapu
-- Date:		03/22/2016
--
-- Purpose:		Update Entity Signatures
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/22/2016	 Sumana Sangapu  Initial creation.
-- 03/25/2016 - Karl Jablonski - Removed erronous params
-- 01/11/2017	Scott Martin	Updated the audit procs to save UserID
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ESignature].[usp_UpdateEntitySingatures]
	@EntitySignatureID bigint,
	@EntityID bigint,
	@SignatureID bigint,
	@EntityTypeID int,
	@SignatureTypeID int,
	@CredentialID bigint,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT 
AS

BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

  SELECT
    @ResultCode = 0,
    @ResultMessage = 'Executed Successfully'

  BEGIN TRY
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'ESignature', 'EntitySignatures', @EntitySignatureID, NULL, NULL, NULL, @EntityID, @EntityTypeID, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

 		UPDATE	[ESignature].[EntitySignatures]  
		SET		
				EntityID = @EntityID,
				SignatureID = @SignatureID,
				EntityTypeID = @EntityTypeID,
				SignatureTypeID = @SignatureTypeID,
				CredentialID = @CredentialID,
				IsActive = 1, 
				ModifiedBy = @ModifiedBy, 
				ModifiedOn = @ModifiedOn, 
				SystemModifiedOn = GETUTCDATE() 
		WHERE	EntitySignatureID = @EntitySignatureID;
  
	EXEC Auditing.usp_AddPostAuditLog 'Update', 'ESignature', 'EntitySignatures', @AuditDetailID, @EntitySignatureID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
  
  END TRY

  BEGIN CATCH
    SELECT
      @ResultCode = ERROR_SEVERITY(),
      @ResultMessage = ERROR_MESSAGE()
  END CATCH

END
