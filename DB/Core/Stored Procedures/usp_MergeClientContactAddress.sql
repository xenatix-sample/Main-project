-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_MergeClientContactAddress
-- Author:		John Crossen
-- Date:		08/03/2016
--
-- Purpose:		Main procedure for Client Contact Addresses
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

CREATE PROCEDURE [Core].[usp_MergeClientContactAddress]
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
		CA.ContactID,
		CA.AddressID,
		CA.MailPermissionID,
		CAST(CASE WHEN CA.ContactID = @ChildID THEN 0 ELSE CA.IsPrimary END AS BIT) AS IsPrimary,
		CA.EffectiveDate,
		CA.ExpirationDate,
		A.Line1,
		A.StateProvince,
		A.County,
		A.Zip,
		0 AS IsDuplicate
	INTO #Addresses
	FROM
		Registration.ContactAddress CA
		INNER JOIN Core.Addresses A
			ON CA.AddressID = A.AddressID
	WHERE
		CA.ContactID IN (@ParentID, @ChildID)
		AND CA.IsActive = 1

	UPDATE Parent
	SET IsDuplicate = 1
	FROM
		#Addresses Parent
		INNER JOIN #Addresses Child
			ON Parent.Line1 = Child.Line1
			AND Parent.StateProvince = Child.StateProvince
			AND Parent.Zip = Child.Zip
			AND ISNULL(Parent.County, 0) = ISNULL(Child.County, 0)
			AND Parent.ContactID <> Child.ContactID
	WHERE
		Parent.Line1 = Child.Line1
		AND Parent.StateProvince = Child.StateProvince
		AND Parent.Zip = Child.Zip
		AND ISNULL(Parent.County, 0) = ISNULL(Child.County, 0)
		AND Parent.ContactID <> Child.ContactID
		AND Parent.ContactID <> @ParentID
		AND Parent.ExpirationDate IS NULL
		AND Child.ExpirationDate IS NULL

	DECLARE @IDS TABLE (PrimaryKey BIGINT, Completed BIT);

	INSERT INTO Registration.ContactAddress
	(
		ContactID,
		AddressID,
		MailPermissionID,
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
		Inserted.ContactAddressID,
		0
	INTO
		@IDS
	SELECT
		@ContactID,
		AddressID,
		MailPermissionID,
		IsPrimary,
		EffectiveDate,
		ExpirationDate,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		#Addresses
	WHERE
		IsDuplicate = 0;

	DECLARE @PKID BIGINT, @Loop INT

	SELECT @Loop = COUNT(*) from @IDS WHERE Completed = 0;
	SELECT @TotalRecords = COUNT(*) from #Addresses;
	SET @TotalRecordsMerged = @Loop;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = PrimaryKey FROM @IDS where Completed = 0;
	SET ROWCOUNT 0
		
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'ContactAddress', @PKID, NULL, @TransactionLogID, 48, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'ContactAddress', @AuditDetailID, @PKID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1;

	UPDATE @IDS
	SET Completed = 1
	WHERE PrimaryKey = @PKID

	END

	DROP TABLE #Addresses;
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END