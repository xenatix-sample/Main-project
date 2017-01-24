-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_MergeContactAlias
-- Author:		Scott Martin
-- Date:		12/14/2016
--
-- Purpose:		Main procedure for Client Contact Alias
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/14/2016	Scott Martin	Initial creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_MergeContactAlias]
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
			@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);	

	-- Get the records from both the Parent and Child and create new records for the new merged Contact
	SELECT
		CA.ContactID,
		CA.AliasFirstName,
		CA.AliasMiddle,
		CA.AliasLastName,
		CA.SuffixID,
		0 AS IsDuplicate
	INTO #Alias
	FROM
		Registration.ContactAlias CA
	WHERE
		CA.ContactID IN (@ParentID, @ChildID)
		AND CA.IsActive = 1

	UPDATE Parent
	SET IsDuplicate = 1
	FROM
		#Alias Parent
		INNER JOIN #Alias Child
			ON Parent.AliasFirstName = Child.AliasFirstName
			AND Parent.AliasMiddle = Child.AliasMiddle
			AND Parent.AliasLastName = Child.AliasLastName
			AND Parent.SuffixID = Child.SuffixID
			AND Parent.ContactID <> Child.ContactID
	WHERE
		Parent.AliasFirstName = Child.AliasFirstName
		AND Parent.AliasMiddle = Child.AliasMiddle
		AND Parent.AliasLastName = Child.AliasLastName
		AND Parent.SuffixID = Child.SuffixID
		AND Parent.ContactID <> Child.ContactID
		AND Parent.ContactID <> @ParentID;

	DECLARE @IDS TABLE (PrimaryKey BIGINT, Completed BIT);
	
	INSERT INTO Registration.ContactAlias
	(
		ContactID,
		AliasFirstName,
		AliasMiddle,
		AliasLastName,
		SuffixID,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	OUTPUT
		Inserted.ContactAliasID,
		0
	INTO
		@IDS
	SELECT
		@ContactID,
		AliasFirstName,
		AliasMiddle,
		AliasLastName,
		SuffixID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		#Alias
	WHERE
		IsDuplicate = 0;

	DECLARE @PKID BIGINT, @Loop INT

	SELECT @Loop = COUNT(*) FROM @IDS WHERE Completed = 0;
	SELECT @TotalRecords = COUNT(*) FROM #Alias;
	SET @TotalRecordsMerged = @Loop;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = PrimaryKey FROM @IDS WHERE Completed = 0;
	SET ROWCOUNT 0
		
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'ContactAlias', @PKID, NULL, @TransactionLogID, 23, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'ContactAlias', @AuditDetailID, @PKID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1
	UPDATE @IDS
	SET Completed = 1
	WHERE
		PrimaryKey = @PKID;
	END

	DROP TABLE #Alias
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END