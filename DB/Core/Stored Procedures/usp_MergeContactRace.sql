-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_MergeContactRace
-- Author:		Scott Martin
-- Date:		12/14/2016
--
-- Purpose:		Main procedure for Client Contact Race
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/14/2016	Scott Martin	Initial creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_MergeContactRace]
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
		CR.ContactID,
		CR.RaceID,
		0 AS IsDuplicate
	INTO #Race
	FROM
		Registration.ContactRace CR
	WHERE
		CR.ContactID IN (@ParentID, @ChildID)
		AND CR.IsActive = 1

	UPDATE Parent
	SET IsDuplicate = 1
	FROM
		#Race Parent
		INNER JOIN #Race Child
			ON Parent.RaceID = Child.RaceID
			AND Parent.ContactID <> Child.ContactID
	WHERE
		Parent.RaceID = Child.RaceID
		AND Parent.ContactID <> Child.ContactID
		AND Parent.ContactID <> @ParentID;

	DECLARE @IDS TABLE (PrimaryKey BIGINT, Completed BIT);
	
	INSERT INTO Registration.ContactRace
	(
		ContactID,
		RaceID,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	OUTPUT
		Inserted.ContactRaceID,
		0
	INTO
		@IDS
	SELECT
		@ContactID,
		RaceID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		#Race
	WHERE
		IsDuplicate = 0;

	DECLARE @PKID BIGINT, @Loop INT

	SELECT @Loop = COUNT(*) FROM @IDS WHERE Completed = 0;
	SELECT @TotalRecords = COUNT(*) FROM #Race;
	SET @TotalRecordsMerged = @Loop;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = PrimaryKey FROM @IDS WHERE Completed = 0;
	SET ROWCOUNT 0
		
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'ContactRace', @PKID, NULL, @TransactionLogID, 24, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'ContactRace', @AuditDetailID, @PKID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1
	UPDATE @IDS
	SET Completed = 1
	WHERE
		PrimaryKey = @PKID;
	END

	DROP TABLE #Race
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END