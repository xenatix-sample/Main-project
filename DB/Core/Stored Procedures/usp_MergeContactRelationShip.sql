-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_MergeContactRelationShip
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
-- 09/07/2016	Scott Martin	Added in logic that will convert the contact type if merging from ECI to Non-ECI contact or vice versa
-- 11/02/2016	Scott Martin	Added logic that will update ChildContactID to ParentID for records the child was a collateral for
-- 12/05/2016	Scott Martin	Refactored proc to copy records from Parent/Child to new merged contact
-- 01/11/2016	Scott Martin	Removed 'ECI' and pointed to DetailID instead
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].usp_MergeContactRelationShip
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
			@ContactTypeID INT,
			@ID BIGINT

	SELECT
		@ContactTypeID = CASE WHEN OD.DetailID IN (3) THEN 8 ELSE 4 END
	FROM
		Registration.Contact C
		LEFT OUTER JOIN Reference.ClientType CT
			ON CT.ClientTypeID = C.ClientTypeID
		LEFT OUTER JOIN Core.OrganizationDetails OD
			ON OD.DetailID = CT.OrganizationDetailID
	WHERE
		C.ContactID = @ParentID;

	SELECT
		CR.ContactRelationshipID,
		CR.ParentContactID AS ContactID,
		C.FirstName,
		C.LastName,
		C.DOB,
		CR.CollateralExpirationDate AS ExpirationDate,
		CAST(0 AS BIGINT) AS NewContactRelationshipID,
		0 AS IsDuplicate,
		0 AS Completed
	INTO #Relationhips
	FROM
		Registration.ContactRelationship CR
		INNER JOIN Registration.Contact C
			ON CR.ChildContactID = C.ContactID
	WHERE
		CR.ParentContactID IN (@ParentID, @ChildID)
		AND CR.IsActive = 1;

	UPDATE Parent
	SET IsDuplicate = 1,
		Completed = 1
	FROM
		#Relationhips Parent
		INNER JOIN #Relationhips Child
			ON Parent.FirstName = Child.FirstName
			AND Parent.LastName = Child.LastName
			AND Parent.DOB = Child.DOB
			AND Parent.ContactID <> Child.ContactID
	WHERE
		Parent.FirstName = Child.FirstName
		AND Parent.LastName = Child.LastName
		AND Parent.DOB = Child.DOB
		AND Parent.ContactID <> Child.ContactID
		AND Parent.ContactID <> @ParentID
		AND Parent.ExpirationDate IS NULL
		AND Child.ExpirationDate IS NULL;

	DECLARE @PKID BIGINT, @Loop INT

	SELECT @Loop = COUNT(*) from #Relationhips WHERE Completed = 0;
	SELECT @TotalRecords = COUNT(*) from #Relationhips;
	SET @TotalRecordsMerged = @Loop;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = ContactRelationshipID FROM #Relationhips WHERE Completed = 0;
	SET ROWCOUNT 0

	INSERT INTO Registration.ContactRelationship
	(
		ParentContactID,
		ChildContactID,
		ContactTypeID,
		PhonePermissionID,
		EmailPermissionID,
		ReceiveCorrespondenceID,
		IsEmergency,
		EducationStatusID,
		SchoolAttended,
		SchoolBeginDate,
		SchoolEndDate,
		EmploymentStatusID,
		VeteranStatusID,
		LivingWithClientStatus,
		CollateralEffectiveDate,
		CollateralExpirationDate,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	SELECT
		@ContactID,
		ChildContactID,
		@ContactTypeID,
		PhonePermissionID,
		EmailPermissionID,
		ReceiveCorrespondenceID,
		IsEmergency,
		EducationStatusID,
		SchoolAttended,
		SchoolBeginDate,
		SchoolEndDate,
		EmploymentStatusID,
		VeteranStatusID,
		LivingWithClientStatus,
		CollateralEffectiveDate,
		CollateralExpirationDate,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		Registration.ContactRelationship CR
		INNER JOIN #Relationhips R
			ON CR.ContactRelationshipID = R.ContactRelationshipID
	WHERE
		CR.ContactRelationshipID = @PKID;

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'ContactRelationship', @ID, NULL, @TransactionLogID, 26, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'ContactRelationship', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1;
	UPDATE #Relationhips
	SET Completed = 1,
		NewContactRelationshipID = @ID
	WHERE
		ContactRelationshipID = @PKID;
	END

	--Copy associated relationship types
	DECLARE @NewContactRelationshipID BIGINT;

	DECLARE @IDstoCopy TABLE
	(
		PKID bigint,
		ContactRelationshipID bigint,
		completed tinyint DEFAULT(0)
	);

	INSERT INTO @IDstoCopy
	SELECT
		CRT.ContactRelationshipTypeID,
		R.NewContactRelationshipID,
		0
	FROM
		Registration.ContactRelationshipType CRT
		INNER JOIN #Relationhips R
			ON CRT.ContactRelationshipID = R.ContactRelationshipID
	WHERE
		R.NewContactRelationshipID > 0;

	SELECT @Loop = COUNT(*) FROM @IDstoCopy WHERE completed = 0;
	SET @TotalRecords = @TotalRecords + @Loop;
	SET @TotalRecordsMerged = @TotalRecordsMerged + @Loop;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT
		@PKID = PKID,
		@NewContactRelationshipID = ContactRelationshipID
		FROM @IDstoCopy WHERE completed = 0;
	SET ROWCOUNT 0

	INSERT INTO Registration.ContactRelationshipType
	(
		ContactRelationshipID,
		RelationshipTypeID,
		IsPolicyHolder,
		OtherRelationship,
		EffectiveDate,
		ExpirationDate,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	SELECT
		@NewContactRelationshipID,
		RelationshipTypeID,
		IsPolicyHolder,
		OtherRelationship,
		EffectiveDate,
		ExpirationDate,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		Registration.ContactRelationshipType CRT
		INNER JOIN @IDstoCopy ITC
			ON CRT.ContactRelationshipTypeID = ITC.PKID
	WHERE
		ITC.PKID = @PKID;

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'ContactRelationshipType', @ID, NULL, @TransactionLogID, 26, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'ContactRelationshipType', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1;
	UPDATE @IDstoCopy
	SET completed = 1
	WHERE
		PKID = @PKID;
	END

	--Update any contact relationship records where Parent/Child record is a collateral for another contact
	DECLARE @OtherContactID BIGINT;

	DECLARE @IDstoUpdate TABLE
	(
		PKID bigint,
		ContactID bigint,
		completed tinyint DEFAULT(0) 
	);

	INSERT INTO @IDstoUpdate
	SELECT
		CR.ContactRelationshipID,
		ParentContactID,
		0
	FROM
		Registration.ContactRelationship CR
	WHERE
		CR.ChildContactID IN (@ParentID, @ChildID)
		AND CR.ParentContactID NOT IN (@ParentID, @ChildID);
	 
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
		@IDstoUpdate
	WHERE
		completed = 0;
	SET ROWCOUNT 0

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ContactRelationship', @PKID, NULL, @TransactionLogID, 26, @OtherContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Registration.ContactRelationship
	SET ChildContactID = @ContactID,
		ModifiedOn = @ModifiedOn,
		ModifiedBy = @ModifiedBy,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ContactRelationshipID = @PKID;
		
	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ContactRelationship', @AuditDetailID, @PKID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1;
	UPDATE @IDstoUpdate
	SET completed = 1
	WHERE
		PKID = @PKID;
	END
	
	DROP TABLE #Relationhips
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
