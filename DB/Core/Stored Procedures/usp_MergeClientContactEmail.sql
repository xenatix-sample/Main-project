-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_MergeClientContactEmail
-- Author:		John Crossen
-- Date:		08/03/2016
--
-- Purpose:		Main procedure for Client Contact Email
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/03/2016 - Initial procedure creation
-- 08/16/2016	Scott Martin	Refactored proc to include auditing and storing results of merging
-- 12/05/2016	Scott Martin	Refactored proc to copy records from Parent/Child to new merged contact 
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_MergeClientContactEmail]
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
		CE.ContactID,
		CE.EmailID,
		CE.EmailPermissionID,
		CAST(CASE WHEN CE.ContactID = @ChildID THEN 0 ELSE CE.IsPrimary END AS BIT) AS IsPrimary,
		CE.EffectiveDate,
		CE.ExpirationDate,
		E.Email,
		0 AS IsDuplicate
	INTO #Emails
	FROM
		Registration.ContactEmail CE
		INNER JOIN Core.Email E
			ON CE.EmailID = E.EmailID
	WHERE
		CE.ContactID IN (@ParentID, @ChildID)
		AND CE.IsActive = 1

	UPDATE Parent
	SET IsDuplicate = 1
	FROM
		#Emails Parent
		INNER JOIN #Emails Child
			ON Parent.Email = Child.Email
			AND Parent.ContactID <> Child.ContactID
	WHERE
		Parent.Email = Child.Email
		AND Parent.ContactID <> Child.ContactID
		AND Parent.ContactID <> @ParentID
		AND Parent.ExpirationDate IS NULL
		AND Child.ExpirationDate IS NULL;

	DECLARE @IDS TABLE (PrimaryKey BIGINT, Completed BIT);
	
	INSERT INTO Registration.ContactEmail
	(
		ContactID,
		EmailID,
		EmailPermissionID,
		IsPrimary,
		EffectiveDate,
		ExpirationDate,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	OUTPUT
		Inserted.ContactEmailID,
		0
	INTO
		@IDS
	SELECT
		@ContactID,
		EmailID,
		EmailPermissionID,
		CASE
			WHEN ContactID = @ChildID THEN 0
			ELSE IsPrimary END AS IsPrimary,
		EffectiveDate,
		ExpirationDate,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		#Emails
	WHERE
		IsDuplicate = 0

	DECLARE @PKID BIGINT, @Loop INT

	SELECT @Loop = COUNT(*) FROM @IDS WHERE Completed = 0;
	SELECT @TotalRecords = COUNT(*) FROM #Emails;
	SET @TotalRecordsMerged = @Loop;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = PrimaryKey FROM @IDS WHERE Completed = 0;
	SET ROWCOUNT 0
		
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'ContactEmail', @PKID, NULL, @TransactionLogID, 50, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'ContactEmail', @AuditDetailID, @PKID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1
	UPDATE @IDS
	SET Completed = 1
	WHERE
		PrimaryKey = @PKID;
	END

	DROP TABLE #Emails
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END