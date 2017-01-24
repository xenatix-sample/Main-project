-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddSignatures]
-- Author:		Sumana Sangapu
-- Date:		08/18/2015
--
-- Purpose:		Add Signatures  
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/18/2015	Sumana Sangapu	914 - Initial Creation
-- 08/20/2015   Justin Spalti - Added the IsActive column and updated the schema to ESignature
-- 09/23/2015   John Crossen        Add Audit
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added CreatedBy and CreatedOn fields
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ESignature].[usp_AddSignatures]
	@SignatureBLOB VARBINARY(max),
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

	INSERT INTO [ESignature].[Signatures]
	(
		SignatureBLOB,
		ModifiedOn,
		ModifiedBy,
		IsActive,
		CreatedBy,
		CreatedOn
	)
	VALUES
	(
		@SignatureBLOB,
		@ModifiedOn,
		@ModifiedBy,
		1,
		@ModifiedBy,
		@ModifiedOn
	);
		
	SELECT @ID = SCOPE_IDENTITY();

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'ESignature', 'Signatures', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'ESignature', 'Signatures', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
  
	END TRY

	BEGIN CATCH
		SELECT	@ID= 0,
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE();
	END CATCH
END