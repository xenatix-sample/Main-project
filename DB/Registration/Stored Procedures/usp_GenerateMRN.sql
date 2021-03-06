-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_GenerateMRN
-- Author:		Sumana Sangapu
-- Date:		10/08/2015
--
-- Purpose:		Generate and return the MRN for the contact 
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/08/2015	Sumana Sangapu	TFS:2662	Initial Creation
-- 01/13/2016   Justin Spalti - Added the ModifiedBy column to the ContactMRN insert statement.
-- 01/14/2016	Scott Martin	added SystemModifiedOn field, added CreatedBy and CreatedOn to Insert
-- [Registration].[usp_GenerateMRN] 1,'','',''
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_GenerateMRN]
	@ContactID Bigint,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS 
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@MRN BIGINT;

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY

	INSERT INTO [Registration].[ContactMRN]
	(
		ContactID,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	VALUES
	(
		@ContactID,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);

	SELECT @MRN = SCOPE_IDENTITY();

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'Contact', @ContactID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	-- Update MRN
	UPDATE Registration.Contact
	SET	MRN = @MRN,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ContactID = @ContactID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'Contact', @AuditDetailID, @ContactID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;			

	END TRY

	BEGIN CATCH
		SELECT
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH

END