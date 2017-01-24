-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_MergeContactPayor
-- Author:		John Crossen
-- Date:		08/03/2016
--
-- Purpose:		Main procedure for Client Eligibility
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/10/2016 - Initial procedure creation
-- 08/16/2016	Scott Martin	Refactored proc to include auditing and storing results of merging
-- 09/06/2016	Scott Martin	Data type for GroupID was incorrect
-- 09/07/2016	Scott Martin	Fixed an issue where child Payor policy holder was listed as self and PolicyHolderID was not updated to match ContactID
-- 11/02/2016	Scott Martin	Added logic that will update PolicyHolderID to ParentID for records the child was a policy holder for
-- 12/05/2016	Scott Martin	Refactored proc to copy records from Parent/Child to new merged contact
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].usp_MergeContactPayor
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
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@FirstName NVARCHAR(200),
		@MiddleName NVARCHAR(200),
		@LastName NVARCHAR(200),
		@Suffix INT

	SELECT
		@FirstName = FirstName,
		@MiddleName = Middle,
		@LastName = LastName,
		@Suffix = SuffixID
	FROM
		Registration.Contact
	WHERE
		ContactID = @ContactID;

	-- Get the records from both the Parent and Child and create new records for the new merged Contact
	SELECT
		CP.ContactPayorID,
		CP.ContactID,
		CP.PayorID,
		CP.GroupID,
		CP.PayorPlanID,
		CP.PolicyID,
		CP.EffectiveDate,
		CP.ExpirationDate,
		0 AS IsDuplicate
	INTO #Payors
	FROM
		Registration.ContactPayor CP
	WHERE
		CP.ContactID IN (@ParentID, @ChildID)
		AND CP.IsActive = 1

	UPDATE Parent
	SET IsDuplicate = 1
	FROM
		#Payors Parent
		INNER JOIN #Payors Child
			ON Parent.PayorID = Child.PayorID
			AND Parent.PolicyID = Child.PolicyID
			AND Parent.PayorPlanID = Child.PayorPlanID
			AND Parent.GroupID = Child.GroupID
			AND Parent.ContactID <> Child.ContactID
	WHERE
		Parent.PayorID = Child.PayorID
		AND Parent.PolicyID = Child.PolicyID
		AND Parent.PayorPlanID = Child.PayorPlanID
		AND Parent.GroupID = Child.GroupID
		AND Parent.ContactID <> Child.ContactID
		AND Parent.ContactID <> @ParentID
		AND Parent.ExpirationDate IS NULL
		AND Child.ExpirationDate IS NULL;

	DECLARE @IDS TABLE (PrimaryKey BIGINT, Completed BIT);

	INSERT INTO [Registration].[ContactPayor]
	(
		[ContactID],
		[PayorID],
		PayorPlanID,
		PayorGroupID,
		[PolicyHolderID],
		[PolicyHolderName],
		[GroupID],
		[PolicyHolderFirstName],
		[PolicyHolderMiddleName],
		[PolicyHolderLastName],
		[PolicyHolderEmployer],
		[PolicyHolderSuffixID],
		[BillingFirstName],
		[BillingMiddleName],
		[BillingLastName],
		[BillingSuffixID],
		[AdditionalInformation],
		[ContactPayorRank],
		[PolicyID],
		[Deductible],
		[Copay],
		[CoInsurance],
		PayorAddressID,
		[EffectiveDate],
		[ExpirationDate],
		[PayorExpirationReasonID],
		[ExpirationReason],
		[AddRetroDate],
		[HasPolicyHolderSameCardName],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn],
		CreatedBy,
		CreatedOn
	)
	OUTPUT
		Inserted.ContactPayorID,
		0
	INTO
		@IDS
	SELECT
		@ContactID,
		CP.PayorID,
		CP.PayorPlanID,
		CP.PayorGroupID,
		CASE WHEN CP.ContactID = CP.PolicyHolderID THEN @ContactID ELSE CP.PolicyHolderID END,
		CP.PolicyHolderName,
		CP.GroupID,
		CASE WHEN CP.ContactID = CP.PolicyHolderID THEN @FirstName ELSE CP.PolicyHolderFirstName END,
		CASE WHEN CP.ContactID = CP.PolicyHolderID THEN @MiddleName ELSE CP.PolicyHolderMiddleName END,
		CASE WHEN CP.ContactID = CP.PolicyHolderID THEN @LastName ELSE CP.PolicyHolderLastName END,
		CP.PolicyHolderEmployer,
		CASE WHEN CP.ContactID = CP.PolicyHolderID THEN @Suffix ELSE CP.PolicyHolderSuffixID END,
		CP.BillingFirstName,
		CP.BillingMiddleName,
		CP.BillingLastName,
		CP.BillingSuffixID,
		CP.AdditionalInformation,
		CP.ContactPayorRank,
		CP.PolicyID,
		CP.Deductible,
		CP.Copay,
		CP.CoInsurance,
		CP.PayorAddressID,
		CP.EffectiveDate,
		CP.ExpirationDate,
		CP.PayorExpirationReasonID,
		CP.ExpirationReason,
		CP.AddRetroDate,
		CP.HasPolicyHolderSameCardName,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		Registration.ContactPayor CP
		INNER JOIN #Payors P
			ON CP.ContactPayorID = P.ContactPayorID
	WHERE
		P.IsDuplicate = 0

	DECLARE @PKID BIGINT, @Loop INT;

	SELECT @Loop = COUNT(*) FROM @IDS WHERE Completed = 0;
	SELECT @TotalRecords = COUNT(*) from #Payors;
	SET @TotalRecordsMerged = @Loop;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = PrimaryKey FROM @IDS WHERE Completed = 0;
	SET ROWCOUNT 0
		
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'ContactPayor', @PKID, NULL, @TransactionLogID, 33, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'ContactPayor', @AuditDetailID, @PKID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1;
	UPDATE @IDS
	SET Completed = 1
	WHERE
		PrimaryKey = @PKID;
	END

	--Update Contact Payor where Policy Holder is child to parent
	DECLARE @OtherContactID BIGINT

	DECLARE @IDstoUpdate TABLE
	(
		PKID bigint,
		ContactID bigint,
		completed tinyint DEFAULT(0) 
	);

	INSERT INTO @IDstoUpdate
	SELECT
		CP.ContactPayorID,
		ContactID,
		0
	FROM
		Registration.ContactPayor CP
	WHERE
		PolicyHolderID IN (@ParentID, @ChildID)
		AND CP.ContactID NOT IN (@ParentID, @ChildID);

	SELECT @Loop = COUNT(*) from @IDstoUpdate WHERE completed = 0;
	SET @TotalRecords = @TotalRecords + @Loop;
	SET @TotalRecordsMerged = @TotalRecordsMerged + @Loop;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT
		@PKID = PKID,
		@OtherContactID = ContactID
	FROM
		@IDstoUpdate WHERE completed = 0;
	SET ROWCOUNT 0
		
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ContactPayor', @PKID, NULL, @TransactionLogID, 33, @OtherContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Registration.ContactPayor
	SET PolicyHolderID = @ContactID,
		PolicyHolderFirstName = @FirstName,
		PolicyHolderMiddleName = @MiddleName,
		PolicyHolderLastName = @LastName,
		PolicyHolderSuffixID = @Suffix,
		ModifiedOn = @ModifiedOn,
		ModifiedBy = @ModifiedBy,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ContactPayorID = @PKID;
		
	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ContactPayor', @AuditDetailID, @PKID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1;
	UPDATE @IDstoUpdate
	SET completed = 1
	WHERE
		PKID = @PKID;
	END
	
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
