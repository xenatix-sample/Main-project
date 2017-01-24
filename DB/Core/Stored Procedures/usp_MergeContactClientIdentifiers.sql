-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_MergeContactClientIdentifiers
-- Author:		Scott Martin
-- Date:		10/14/2016
--
-- Purpose:		Main procedure for Client Identifier
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/10/2016	Scott Martin	Initial procedure creation
-- 12/05/2016	Scott Martin	Refactored proc to copy records from Parent/Child to new merged contact 
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].usp_MergeContactClientIdentifiers
(
	@TransactionLogID BIGINT,
	@ContactID BIGINT,
	@ParentID BIGINT,
	@ChildID BIGINT,
	@TotalRecords INT OUTPUT,
	@TotalRecordsMerged INT OUTPUT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
)
AS
BEGIN
	SELECT @ResultCode = 0,
			@ResultMessage = 'Data saved successfully'

	BEGIN TRY	
	DECLARE @AuditDetailID BIGINT,
			@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID)

	-- Get the records from both the Parent and Child and create new records for the new merged Contact
	DECLARE @IDS TABLE (PrimaryKey BIGINT, Completed BIT)

	INSERT INTO Registration.ContactClientIdentifier
	(
		ContactID,
		ClientIdentifierTypeID,
		AlternateID,
		ExpirationReasonID,
		EffectiveDate,
		ExpirationDate,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	OUTPUT
		Inserted.ContactClientIdentifierID,
		0
	INTO
		@IDS
	SELECT
		@ContactID,
		ClientIdentifierTypeID,
		AlternateID,
		ExpirationReasonID,
		EffectiveDate,
		ExpirationDate,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		Registration.ContactClientIdentifier CCI
	WHERE
		CCI.ContactID IN (@ParentID, @ChildID)
		AND IsActive = 1

	DECLARE @PKID BIGINT, @Loop INT

	SELECT @Loop = COUNT(*) FROM @IDS;
	SET @TotalRecords = @Loop;
	SET @TotalRecordsMerged = @Loop;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = PrimaryKey FROM @IDS WHERE Completed = 0;
	SET ROWCOUNT 0
		
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'ContactClientIdentifier', @PKID, NULL, @TransactionLogID, 23, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'ContactClientIdentifier', @AuditDetailID, @PKID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1;
	UPDATE @IDS
	SET Completed = 1
	WHERE
		PrimaryKey = @PKID;
	END
	
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END