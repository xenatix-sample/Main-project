-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_MergeClientContactPhone
-- Author:		John Crossen
-- Date:		08/03/2016
--
-- Purpose:		Main procedure for Client Contact Phone
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


CREATE PROCEDURE [Core].usp_MergeClientContactPhone
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
	SELECT
		CP.ContactID,
		CP.PhoneID,
		CP.PhonePermissionID,
		CAST(CASE WHEN CP.ContactID = @ChildID THEN 0 ELSE CP.IsPrimary END AS BIT) AS IsPrimary,
		CP.EffectiveDate,
		CP.ExpirationDate,
		P.Number,
		0 AS IsDuplicate
	INTO #Phones
	FROM
		Registration.ContactPhone CP
		INNER JOIN Core.Phone P
			ON CP.PhoneID = P.PhoneID
	WHERE
		CP.ContactID IN (@ParentID, @ChildID)
		AND CP.IsActive = 1

	UPDATE Parent
	SET IsDuplicate = 1
	FROM
		#Phones Parent
		INNER JOIN #Phones Child
			ON Parent.Number = Child.Number
			AND Parent.ContactID <> Child.ContactID
	WHERE
		Parent.Number = Child.Number
		AND Parent.ContactID <> Child.ContactID
		AND Parent.ContactID <> @ParentID
		AND Parent.ExpirationDate IS NULL
		AND Child.ExpirationDate IS NULL;

	DECLARE @IDS TABLE (PrimaryKey BIGINT, Completed BIT);

	INSERT INTO Registration.ContactPhone
	(
		ContactID,
		PhoneID,
		PhonePermissionID,
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
		Inserted.ContactPhoneID,
		0
	INTO
		@IDS
	SELECT
		@ContactID,
		PhoneID,
		PhonePermissionID,
		IsPrimary,
		EffectiveDate,
		ExpirationDate,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		#Phones
	WHERE
		IsDuplicate = 0;

	DECLARE @PKID BIGINT, @Loop INT

	SELECT @Loop = COUNT(*) FROM @IDS WHERE Completed = 0;
	SELECT @TotalRecords = COUNT(*) FROM #Phones;
	SET @TotalRecordsMerged = @Loop;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = PrimaryKey FROm @IDS WHERE Completed = 0;
	SET ROWCOUNT 0
		
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'ContactPhone', @PKID, NULL, @TransactionLogID, 49, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'ContactPhone', @AuditDetailID, @PKID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop-1
	
	UPDATE @IDS
	SET Completed = 1
	WHERE
		PrimaryKey = @PKID;
	END

	DROP TABLE #Phones
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END