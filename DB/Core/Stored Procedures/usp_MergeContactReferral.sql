-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_MergeContactReferral
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
-- 12/05/2016	Scott Martin	Refactored proc to copy records from Parent/Child to new merged contact 
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].usp_MergeContactReferral
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
			@ID BIGINT;

	-- Get the records from both the Parent and Child and create new records for the new merged Contact
	SELECT
		RAD.ReferralAdditionalDetailID,
		CAST(0 AS BIGINT) AS NewReferralAdditionalDetailID,
		RAD.ReferralHeaderID,
		0 AS Completed
	INTO
		#Referrals
	FROM
		Registration.ReferralAdditionalDetails RAD
		INNER JOIN Registration.ReferralHeader RH
			ON RAD.ReferralHeaderID = RH.ReferralHeaderID
	WHERE
		RAD.ContactID IN (@ParentID, @ChildID)
		AND RAD.IsActive = 1;	

	DECLARE @PKID BIGINT, @Loop INT

	SELECT @Loop = COUNT(*) FROM #Referrals;
	SET @TotalRecords = @Loop;
	SET @TotalRecordsMerged = @Loop;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = ReferralHeaderID FROM #Referrals WHERE Completed = 0;
	SET ROWCOUNT 0

	INSERT INTO Registration.ReferralHeader
	(
		ContactID,
		ReferralDate,
		ReferralStatusID,
		ReferralTypeID,
		ResourceTypeID,
		ReferralCategorySourceID,
		ReferralOriginID,
		ProgramID,
		ReferralOrganizationID,
		OtherOrganization,
		ReferralSourceID,
		OtherSource,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	SELECT
		ContactID,
		ReferralDate,
		ReferralStatusID,
		ReferralTypeID,
		ResourceTypeID,
		ReferralCategorySourceID,
		ReferralOriginID,
		ProgramID,
		ReferralOrganizationID,
		OtherOrganization,
		ReferralSourceID,
		OtherSource,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		Registration.ReferralHeader RH
		INNER JOIN #Referrals R
			ON RH.ReferralHeaderID = R.ReferralHeaderID
	WHERE
		R.ReferralHeaderID = @PKID;

	SELECT @ID = SCOPE_IDENTITY();
		
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'ReferralHeader', @ID, NULL, @TransactionLogID, 42, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'ReferralHeader', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	INSERT INTO Registration.ReferralAdditionalDetails
	(
		ReferralHeaderID,
		ContactID,
		ReasonforCare,
		IsTransferred,
		IsHousingProgram,
		HousingDescription,
		IsEligibleforFurlough,
		IsReferralDischargeOrTransfer,
		IsConsentRequired,
		Comments,
		AdditionalConcerns,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	SELECT
		@ID,
		@ContactID,
		ReasonforCare,
		IsTransferred,
		IsHousingProgram,
		HousingDescription,
		IsEligibleforFurlough,
		IsReferralDischargeOrTransfer,
		IsConsentRequired,
		Comments,
		AdditionalConcerns,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		Registration.ReferralAdditionalDetails RAD
		INNER JOIN #Referrals R
			ON RAD.ReferralHeaderID = R.ReferralHeaderID
	WHERE
		R.ReferralHeaderID = @PKID;

	SELECT @ID = SCOPE_IDENTITY();
		
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'ReferralAdditionalDetails', @ID, NULL, @TransactionLogID, 42, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'ReferralAdditionalDetails', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1;
	UPDATE #Referrals
	SET Completed = 1,
		NewReferralAdditionalDetailID = @ID
	WHERE
		ReferralHeaderID = @PKID;
	END

	--Copy referral concerns
	DECLARE @IDS TABLE (PrimaryKey BIGINT, Completed BIT);

	INSERT INTO Registration.ReferralConcernDetails
	(
		ReferralAdditionalDetailID,
		ReferralConcernID,
		Diagnosis,
		ReferralPriorityID,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	OUTPUT
		inserted.ReferralConcernDetailID,
		0
	INTO
		@IDS
	SELECT
		NewReferralAdditionalDetailID,
		ReferralConcernID,
		Diagnosis,
		ReferralPriorityID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		Registration.ReferralConcernDetails RCD
		INNER JOIN #Referrals R
			ON RCD.ReferralAdditionalDetailID = R.ReferralAdditionalDetailID
	WHERE
		RCD.IsActive = 1;

	SELECT @Loop = COUNT(*) from @IDS WHERE Completed = 0;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = PrimaryKey FROM @IDS where Completed = 0;
	SET ROWCOUNT 0
		
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'ReferralConcernDetails', @PKID, NULL, @TransactionLogID, 48, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'ReferralConcernDetails', @AuditDetailID, @PKID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1;

	UPDATE @IDS
	SET Completed = 1
	WHERE PrimaryKey = @PKID
	END
	
	--Update any referral header records where contact is a referrer for a referral
	DECLARE @OtherContactID BIGINT;

	DECLARE @IDstoUpdate TABLE
	(
		PKID bigint,
		ContactID bigint,
		completed tinyint DEFAULT(0) 
	);

	INSERT INTO @IDstoUpdate
	SELECT
		RH.ReferralHeaderID,
		RAD.ContactID,
		0
	FROM
		Registration.ReferralHeader RH
		INNER JOIN Registration.ReferralAdditionalDetails RAD
			ON RH.ReferralHeaderID = RAD.ReferralHeaderID
	WHERE
		RH.ContactID IN (@ParentID, @ChildID);
	 
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

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ReferralHeader', @PKID, NULL, @TransactionLogID, 26, @OtherContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Registration.ReferralHeader
	SET ContactID = @ContactID,
		ModifiedOn = @ModifiedOn,
		ModifiedBy = @ModifiedBy,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ReferralHeaderID = @PKID;
		
	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ReferralHeader', @AuditDetailID, @PKID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1;
	UPDATE @IDstoUpdate
	SET completed = 1
	WHERE
		PKID = @PKID;
	END

	DROP TABLE #Referrals
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END