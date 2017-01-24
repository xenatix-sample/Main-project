

-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddEntitySignatures]
-- Author:		Sumana Sangapu
-- Date:		03/22/2016
--
-- Purpose:		Add Signatures to Signature and EntitySignature 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/22/2016	Sumana Sangapu - Initial creation
-- 04/19/2016	Karl Jablonski - Adding SignatureTypeID
-- 01/11/2017	Scott Martin	Updated the audit procs to save UserID
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ESignature].[usp_AddEntitySignatures]
	@EntityID BIGINT,
	@SignatureID INT,
	@EntityTypeID int,
	@SignatureTypeID int,
	@CredentialID bigint,
	@IsActive BIT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT ,
	@ID INT OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	BEGIN TRY	
			
	------ Insert into ESignature.EntitySignatures	
	INSERT INTO ESignature.EntitySignatures
	(
		EntityID,
		SignatureID,
		EntityTypeID,
		SignatureTypeID,
		CredentialID,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	VALUES
	(	@EntityID,
		@SignatureID,
		@EntityTypeID,
		@SignatureTypeID,
		@CredentialID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	)

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'ESignature', 'EntitySignatures', @ID, NULL, NULL, NULL, @EntityID, @EntityTypeID, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'ESignature', 'EntitySignatures', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
		SELECT  @ID = 0,
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE();
	END CATCH
END
