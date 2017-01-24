-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_MergeCallCenterHeader
-- Author:		John Crossen
-- Date:		08/03/2016
--
-- Purpose:		Main procedure for Client Call Center Header
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/10/2016 - Initial procedure creation
-- 08/16/2016	Scott Martin	Refactored proc to include auditing and storing results of merging
-- 12/05/2016	Scott Martin	Refactored proc to copy records from Parent/Child to new merged contact 
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].usp_MergeCallCenterHeader
(
	@TransactionLogID BIGINT,
	@ContactID BIGINT,
	@ParentID BIGINT,
	@ChildID BIGINT,
	@CallCenterTypeID SMALLINT,
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
			@ID BIGINT,
			@ModuleComponentID BIGINT,
			@OtherContactID BIGINT,
			@ServiceRecordingSourceID INT,
			@CallCenterHeaderIDList NVARCHAR(MAX);

	IF @CallCenterTypeID = 1
		BEGIN
		SET @ModuleComponentID = 43;
		SET @ServiceRecordingSourceID = 1;
		END
	
	IF @CallCenterTypeID = 2
		BEGIN
		SET @ModuleComponentID = 44;
		SET @ServiceRecordingSourceID = 6;
		END

	SELECT
		CCH.CallCenterHeaderID,
		CAST(0 AS BIGINT) AS NewCallCenterHeaderID,
		CCH.ContactID,
		0 AS Completed
	INTO
		#CallCenter
	FROM
		CallCenter.CallCenterHeader CCH
	WHERE
		CCH.ContactID IN (@ParentID, @ChildID)
		AND CCH.IsActive = 1
		AND CCH.CallCenterTypeID = @CallCenterTypeID;

	DECLARE @PKID BIGINT, @Loop INT

	SELECT @Loop = COUNT(*) FROM #CallCenter;
	SET @TotalRecords = @Loop;
	SET @TotalRecordsMerged = @Loop;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = CallCenterHeaderID, @OtherContactID = ContactID FROM #CallCenter WHERE Completed = 0
	SET ROWCOUNT 0
	
	INSERT INTO CallCenter.CallCenterHeader
	(
		ParentCallCenterHeaderID,
		CallCenterTypeID,
		CallerID,
		ContactID,
		ContactTypeID,
		ProviderID,
		CallStartTime,
		CallEndTime,
		CallStatusID,
		ProgramUnitID,
		CountyID,
		IsLinkedToContact,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	SELECT
		CCH.ParentCallCenterHeaderID,
		@CallCenterTypeID,
		CCH.CallerID,
		@ContactID,
		CCH.ContactTypeID,
		CCH.ProviderID,
		CCH.CallStartTime,
		CCH.CallEndTime,
		CCH.CallStatusID,
		CCH.ProgramUnitID,
		CCH.CountyID,
		CCH.IsLinkedToContact,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		CallCenter.CallCenterHeader CCH
		INNER JOIN #CallCenter CH
			ON CCH.CallCenterHeaderID = CH.CallCenterHeaderID
	WHERE
		CH.CallCenterHeaderID = @PKID;

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'CallCenter', 'CallCenterHeader', @ID, NULL, @TransactionLogID, @ModuleComponentID, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'CallCenter', 'CallCenterHeader', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'CallCenter', 'CallCenterHeader', @PKID, NULL, @TransactionLogID, @ModuleComponentID, @OtherContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	UPDATE CallCenter.CallCenterHeader
	SET IsActive = 0,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		CallCenterHeaderID = @PKID;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'CallCenter', 'CallCenterHeader', @AuditDetailID, @PKID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;


	INSERT INTO CallCenter.CrisisCall
	(
		CallCenterHeaderID,
		CallCenterPriorityID,
		SuicideHomicideID,
		DateOfIncident,
		ReasonCalled,
		Disposition,
		OtherInformation,
		Comments,
		FollowUpRequired,
		CallTypeID,
		CallTypeOther,
		FollowupPlan,
		NatureofCall,
		ClientStatusID,
		ClientProviderID,
		BehavioralCategoryID,
		NoteHeaderID,
		ReferralAgencyID,
		OtherReferralAgency,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	SELECT
		@ID,
		CallCenterPriorityID,
		SuicideHomicideID,
		DateOfIncident,
		ReasonCalled,
		Disposition,
		OtherInformation,
		Comments,
		FollowUpRequired,
		CallTypeID,
		CallTypeOther,
		FollowupPlan,
		NatureofCall,
		ClientStatusID,
		ClientProviderID,
		BehavioralCategoryID,
		NoteHeaderID,
		ReferralAgencyID,
		OtherReferralAgency,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		CallCenter.CrisisCall CC
		INNER JOIN #CallCenter CH
			ON CC.CallCenterHeaderID = CH.CallCenterHeaderID
	WHERE
		CH.CallCenterHeaderID = @PKID;

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'CallCenter', 'CrisisCall', @ID, NULL, @TransactionLogID, @ModuleComponentID, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'CallCenter', 'CrisisCall', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1;
	UPDATE #CallCenter
	SET Completed = 1,
		NewCallCenterHeaderID = @ID
	WHERE
		CallCenterHeaderID = @PKID;
	END

	--Update Parent header ID
	DECLARE @ParentIDS TABLE (PrimaryKey BIGINT, ParentPrimaryKey BIGINT, Completed BIT);

	INSERT INTO @ParentIDS
	SELECT
		CCH.CallCenterHeaderID,
		CC.NewCallCenterHeaderID AS ParentCallCenterHeaderID,
		0 AS Completed
	FROM
		CallCenter.CallCenterHeader CCH
		INNER JOIN #CallCenter CC
			ON CCH.ParentCallCenterHeaderID = CC.CallCenterHeaderID
	WHERE
		CCH.IsActive = 1

	SELECT @Loop = COUNT(*) FROM @ParentIDS;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = PrimaryKey FROm @ParentIDS WHERE Completed = 0
	SET ROWCOUNT 0

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'CallCenter', 'CallCenterHeader', @PKID, NULL, @TransactionLogID, @ModuleComponentID, @OtherContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	
	UPDATE CCH
	SET	ParentCallCenterHeaderID = ITU.ParentPrimaryKey,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	FROM
		CallCenter.CallCenterHeader CCH
		INNER JOIN @ParentIDS ITU
			ON CCH.CallCenterHeaderID = ITU.PrimaryKey
	WHERE
		CCH.CallCenterHeaderID = ITU.PrimaryKey
		AND ITU.PrimaryKey = @PKID;
		
	EXEC Auditing.usp_AddPostAuditLog 'Update', 'CallCenter', 'CallCenterHeader', @AuditDetailID, @PKID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1;
	UPDATE @ParentIDS
	SET Completed = 1
	WHERE
		PrimaryKey = @PKID;
	END

	--Copy Assessments
	SELECT
		CCAR.CallCenterAssessmentResponseID,
		CC.NewCallCenterHeaderID,
		MAR.NewResponseID,
		0 AS Completed
	INTO
		#Responses
	FROM
		CallCenter.CallCenterAssessmentResponse CCAR
		INNER JOIN #CallCenter CC
			ON CCAR.CallCenterHeaderID = CC.CallCenterHeaderID
		INNER JOIN Core.MergedAssessmentResponse MAR
			ON CCAR.ResponseID = MAR.ResponseID
			AND MAR.TransactionLogID = @TransactionLogID;

	SELECT @Loop = COUNT(*) FROM #Responses;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = CallCenterAssessmentResponseID FROm #Responses WHERE Completed = 0
	SET ROWCOUNT 0
	
	INSERT INTO CallCenter.CallCenterAssessmentResponse
	(
		CallCenterHeaderID,
		AssessmentID,
		ResponseID,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	SELECT
		R.NewCallCenterHeaderID,
		CCAR.AssessmentID,
		R.NewResponseID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		CallCenter.CallCenterAssessmentResponse CCAR
		INNER JOIN #Responses R
			ON CCAR.CallCenterAssessmentResponseID = R.CallCenterAssessmentResponseID
	WHERE
		R.CallCenterAssessmentResponseID = @PKID;

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'CallCenter', 'CallCenterAssessmentResponse', @PKID, NULL, @TransactionLogID, @ModuleComponentID, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'CallCenter', 'CallCenterAssessmentResponse', @AuditDetailID, @PKID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1;
	UPDATE #Responses
	SET Completed = 1
	WHERE
		CallCenterAssessmentResponseID = @PKID;
	END

	--Copy Progress note
	SELECT
		PN.ProgressNoteID,
		CC.NewCallCenterHeaderID,
		0 AS Completed
	INTO
		#Progess
	FROM
		CallCenter.ProgressNote PN
		INNER JOIN #CallCenter CC
			ON PN.CallCenterHeaderID = CC.CallCenterHeaderID;

	SELECT @Loop = COUNT(*) FROM #Progess;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = ProgressNoteID FROm #Progess WHERE Completed = 0
	SET ROWCOUNT 0

	INSERT INTO Registration.NoteHeader
	(
		ContactID,
		NoteTypeID,
		TakenBy,
		TakenTime,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	SELECT
		@ContactID,
		NH.NoteTypeID,
		NH.TakenBy,
		NH.TakenTime,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		Registration.NoteHeader NH
		INNER JOIN CallCenter.ProgressNote PN
			ON NH.NoteHeaderID = PN.NoteHeaderID
		INNER JOIN #Progess P
			ON PN.ProgressNoteID = P.ProgressNoteID
	WHERE
		P.ProgressNoteID = @PKID;

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'NoteHeader', @ID, NULL, @TransactionLogID, @ModuleComponentID, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'NoteHeader', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	
	INSERT INTO CallCenter.ProgressNote
	(
		CallCenterHeaderID,
		NoteHeaderID,
		Disposition,
		Comments,
		CallTypeID,
		CallTypeOther,
		FollowupPlan,
		NatureofCall,
		IsCallerSame,
		NewCallerID,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	SELECT
		P.NewCallCenterHeaderID,
		@ID,
		PN.Disposition,
		PN.Comments,
		PN.CallTypeID,
		PN.CallTypeOther,
		PN.FollowupPlan,
		PN.NatureofCall,
		PN.IsCallerSame,
		PN.NewCallerID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		CallCenter.ProgressNote PN
		INNER JOIN #Progess P
			ON PN.ProgressNoteID = P.ProgressNoteID
	WHERE
		P.ProgressNoteID = @PKID;

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'CallCenter', 'ProgressNote', @ID, NULL, @TransactionLogID, @ModuleComponentID, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'CallCenter', 'ProgressNote', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1;
	UPDATE #Progess
	SET Completed = 1
	WHERE
		ProgressNoteID = @PKID;
	END

	--Update records where Parent/Child are linked on Call Center Header
	DECLARE @IDStoUpdate TABLE (PrimaryKey BIGINT, ContactID BIGINT, Completed BIT);

	INSERT INTO @IDStoUpdate
	SELECT
		CCH.CallCenterHeaderID,
		CCH.ContactID,
		0 AS Completed
	FROM
		CallCenter.CallCenterHeader CCH
	WHERE
		CCH.CallerID IN (@ParentID, @ChildID)
		AND CCH.ContactID NOT IN (@ParentID, @ChildID)
		AND CCH.IsActive = 1;

	SELECT @Loop = COUNT(*) FROM @IDStoUpdate;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = PrimaryKey, @OtherContactID = ContactID FROm @IDStoUpdate WHERE Completed = 0
	SET ROWCOUNT 0

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'CallCenter', 'CallCenterHeader', @PKID, NULL, @TransactionLogID, @ModuleComponentID, @OtherContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	
	UPDATE CCH
	SET	CallerID = @ContactID,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	FROM
		CallCenter.CallCenterHeader CCH
		INNER JOIN @IDStoUpdate ITU
			ON CCH.CallCenterHeaderID = ITU.PrimaryKey
	WHERE
		CCH.CallCenterHeaderID = ITU.PrimaryKey
		AND ITU.PrimaryKey = @PKID;
		
	EXEC Auditing.usp_AddPostAuditLog 'Update', 'CallCenter', 'CallCenterHeader', @AuditDetailID, @PKID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1;
	UPDATE @IDStoUpdate
	SET Completed = 1
	WHERE
		PrimaryKey = @PKID;
	END

	--Update records where Parent/Child are linked on Progress Note
	DELETE FROM @IDStoUpdate;

	INSERT INTO @IDStoUpdate
	SELECT
		PN.ProgressNoteID,
		CCH.ContactID,
		0 AS Completed
	FROM
		CallCenter.ProgressNote PN
		INNER JOIN CallCenter.CallCenterHeader CCH
			ON PN.CallCenterHeaderID = CCH.CallCenterHeaderID
	WHERE
		PN.NewCallerID IN (@ParentID, @ChildID)
		AND PN.IsActive = 1;

	SELECT @Loop = COUNT(*) FROM @IDStoUpdate;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = PrimaryKey, @OtherContactID = ContactID FROM @IDStoUpdate WHERE Completed = 0
	SET ROWCOUNT 0

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'CallCenter', 'ProgressNote', @PKID, NULL, @TransactionLogID, @ModuleComponentID, @OtherContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	
	UPDATE PN
	SET NewCallerID = @ContactID,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	FROM
		CallCenter.ProgressNote PN
		INNER JOIN @IDStoUpdate ITU
			ON PN.ProgressNoteID = ITU.PrimaryKey
	WHERE
		PN.ProgressNoteID = ITU.PrimaryKey
		AND ITU.PrimaryKey = @PKID;
		
	EXEC Auditing.usp_AddPostAuditLog 'Update', 'CallCenter', 'ProgressNote', @AuditDetailID, @PKID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1;
	UPDATE @IDStoUpdate
	SET Completed = 1
	WHERE
		PrimaryKey = @PKID;
	END

	--Copy/Update services
	SELECT @CallCenterHeaderIDList = COALESCE(@CallCenterHeaderIDList + ',' , '') + CONVERT(NVARCHAR(MAX), CallCenterHeaderID) FROM #CallCenter;

	EXEC Auditing.usp_AddServiceRecordingHistory @TransactionLogID, @CallCenterHeaderIDList, @ServiceRecordingSourceID, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	DECLARE @IDS TABLE (PKID bigint, CallCenterHeaderID bigint, Completed tinyint DEFAULT(0));

	INSERT INTO @IDS
	SELECT
		SR.ServiceRecordingID,
		CC.NewCallCenterHeaderID,
		0
	FROM
		Core.ServiceRecording SR
		INNER JOIN #CallCenter CC
			ON SR.SourceHeaderID = CC.CallCenterHeaderID
	WHERE
		SR.ServiceRecordingSourceID = @ServiceRecordingSourceID;
	
	SELECT @Loop = COUNT(*) FROM @IDS WHERE Completed = 0;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = PKID FROM @IDS WHERE Completed = 0;
	SET ROWCOUNT 0
		
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'ServiceRecording', @PKID, NULL, @TransactionLogID, @ModuleComponentID, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	UPDATE Core.ServiceRecording
	SET SourceHeaderID = ITU.CallCenterHeaderID
	FROM
		Core.ServiceRecording SR
		INNER JOIN @IDS ITU
			ON SR.ServiceRecordingID = ITU.PKID
	WHERE
		ITU.PKID = @PKID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'ServiceRecording', @AuditDetailID, @PKID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop-1
	
	UPDATE @IDS
	SET Completed = 1
	WHERE
		PKID = @PKID;
	END

	DROP TABLE #CallCenter
	DROP TABLE #Responses
	DROP TABLE #Progess
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END