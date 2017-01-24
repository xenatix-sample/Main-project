-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddContactSignatures]
-- Author:		Justin Spalti
-- Date:		08/18/2015
--
-- Purpose:		Add Signatures  
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/25/2015	Justin Spalti - Initial creation
-- 09/01/2015   Justin Spalti - Corrected the column sizes to prevent a warning while building
-- 09/23/2015   John Crossen        Add Audit
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added CreatedBy and CreatedOn fields
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ESignature].[usp_AddContactSignatures]
	@ContactID BIGINT,
	@SignatureBLOB VARBINARY(max),
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

	CREATE TABLE #tmpContactSignatures 
	(
		tmpSignatureID INT PRIMARY KEY IDENTITY(1,1),
		SignatureBLOB VARBINARY(6000)
	);

	INSERT INTO #tmpContactSignatures 
	(
		SignatureBLOB
	)
	SELECT @SignatureBLOB;

	CREATE TABLE #tmpSignaturesCreated
	(
		SignatureID BIGINT PRIMARY KEY
	);

	INSERT INTO ESignature.Signatures
	(
		SignatureBLOB,
		ModifiedOn,
		ModifiedBy,
		IsActive,
		CreatedBy,
		CreatedOn
	)
	OUTPUT inserted.SignatureID
	INTO #tmpSignaturesCreated
	SELECT
		t.SignatureBLOB,
		@ModifiedOn,
		@ModifiedBy,
		1,
		@ModifiedBy,
		@ModifiedOn
	FROM
		#tmpContactSignatures t;

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'ESignature', 'Signatures', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'ESignature', 'Signatures', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	INSERT INTO ESignature.ContactSignatures
	(
		ContactID,
		SignatureID,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	SELECT
		@ContactID,
		t.SignatureID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		#tmpSignaturesCreated t;

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'ESignature', 'Signatures', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'ESignature', 'Signatures', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	DROP TABLE #tmpSignaturesCreated;
	DROP TABLE #tmpContactSignatures;
			
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE();
	END CATCH
END
