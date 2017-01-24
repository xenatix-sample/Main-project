-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_MergeContactAdmission
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
-- 12/05/2016	Scott Martin	Refactored proc to copy records from Parent/Child to new merged contact 
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].usp_MergeContactAdmission
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
		CA.ContactAdmissionID,
		CA.EffectiveDate,
		MIN(CA.EffectiveDate) OVER (PARTITION BY CA.OrganizationID) AS MinEffectiveDate,
		0 AS Completed
	INTO
		#ActiveAdmissions
	FROM
		Registration.ContactAdmission CA
		LEFT OUTER JOIN Registration.ContactDischargeNote CDN
			ON CA.ContactAdmissionID = CDN.ContactAdmissionID
	WHERE
		CDN.ContactAdmissionID IS NULL
		AND CA.ContactID IN (@ParentID, @ChildID);

	UPDATE #ActiveAdmissions
	SET Completed = 1
	WHERE
		EffectiveDate > MinEffectiveDate;

	DECLARE @PKID BIGINT, @Loop INT

	SELECT @Loop = COUNT(*) FROM #ActiveAdmissions WHERE Completed = 0;
	SELECT @TotalRecords = COUNT(*) FROM #ActiveAdmissions;
	SET @TotalRecordsMerged = @Loop;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = ContactAdmissionID FROM #ActiveAdmissions WHERE Completed = 0;
	SET ROWCOUNT 0

	INSERT INTO Registration.ContactAdmission
	(
		ContactID,
		OrganizationID,
		EffectiveDate,
		UserID,
		IsDocumentationComplete,
		Comments,
		AdmissionReasonID,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	SELECT
		@ContactID,
		CA.OrganizationID,
		CA.EffectiveDate,
		CA.UserID,
		CA.IsDocumentationComplete,
		CA.Comments,
		CA.AdmissionReasonID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		Registration.ContactAdmission CA
		INNER JOIN #ActiveAdmissions A
			ON CA.ContactAdmissionID = A.ContactAdmissionID
	WHERE
		A.ContactAdmissionID = @PKID;

	SELECT @ID = SCOPE_IDENTITY();
			
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'ContactAdmission', @ID, NULL, @TransactionLogID, 45, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'ContactAdmission', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1;
	UPDATE #ActiveAdmissions
	SET Completed = 1
	WHERE
		ContactAdmissionID = @PKID;
	END

	--Discharges
	SELECT
		CA.ContactAdmissionID,
		CAST(0 AS BIGINT) AS NewContactAdmissionID,
		0 AS Completed
	INTO
		#Discharges
	FROM
		Registration.ContactAdmission CA
		LEFT OUTER JOIN Registration.ContactDischargeNote CDN
			ON CA.ContactAdmissionID = CDN.ContactAdmissionID
	WHERE
		CDN.ContactAdmissionID IS NOT NULL
		AND CA.ContactID IN (@ParentID, @ChildID);

	SELECT @Loop = COUNT(*) FROM #Discharges WHERE Completed = 0;
	SET @TotalRecords = @TotalRecords + (SELECT COUNT(*) FROM #Discharges);
	SET @TotalRecordsMerged = @TotalRecordsMerged + @Loop;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = ContactAdmissionID FROM #Discharges WHERE Completed = 0;
	SET ROWCOUNT 0

	INSERT INTO Registration.ContactAdmission
	(
		ContactID,
		OrganizationID,
		EffectiveDate,
		UserID,
		IsDocumentationComplete,
		Comments,
		AdmissionReasonID,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	SELECT
		@ContactID,
		CA.OrganizationID,
		CA.EffectiveDate,
		CA.UserID,
		CA.IsDocumentationComplete,
		CA.Comments,
		CA.AdmissionReasonID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		Registration.ContactAdmission CA
		INNER JOIN #Discharges D
			ON CA.ContactAdmissionID = D.ContactAdmissionID
	WHERE
		D.ContactAdmissionID = @PKID;

	SELECT @ID = SCOPE_IDENTITY();
			
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'ContactAdmission', @ID, NULL, @TransactionLogID, 45, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'ContactAdmission', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1;
	UPDATE #Discharges
	SET Completed = 1,
		NewContactAdmissionID = @ID
	WHERE
		ContactAdmissionID = @PKID;
	END
	
	--Copy detail records
	DECLARE @NewContactAdmissionID BIGINT;

	DECLARE @IDstoCopy TABLE
	(
		PKID bigint,
		NoteHeaderID bigint,
		NoteHeaderVoidID bigint,
		ContactAdmissionID bigint,
		completed tinyint DEFAULT(0)
	);

	INSERT INTO @IDstoCopy
	SELECT
		CDN.ContactDischargeNoteID,
		NH.NoteHeaderID,
		NHV.NoteHeaderVoidID,
		D.NewContactAdmissionID,
		0
	FROM
		Registration.ContactDischargeNote CDN
		LEFT OUTER JOIN Registration.NoteHeader NH
			ON CDN.NoteHeaderID = NH.NoteHeaderID
		LEFT OUTER JOIN Registration.NoteHeaderVoid NHV
			ON NH.NoteHeaderID = NHV.NoteHeaderVoidID
		INNER JOIN #Discharges D
			ON CDN.ContactAdmissionID = D.ContactAdmissionID;

	SELECT @Loop = COUNT(*) FROM @IDstoCopy WHERE completed = 0;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = PKID FROM @IDstoCopy WHERE completed = 0;
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
		NoteTypeID,
		TakenBy,
		TakenTime,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		Registration.NoteHeader NH
		INNER JOIN @IDstoCopy ITC
			ON NH.NoteHeaderID = ITC.NoteHeaderID
	WHERE
		ITC.PKID = @PKID;

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'NoteHeader', @ID, NULL, @TransactionLogID, 45, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'NoteHeader', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	IF EXISTS (SELECT TOP 1 * FROM @IDstoCopy WHERE PKID = @PKID AND NoteHeaderVoidID IS NOT NULL)
		BEGIN
		INSERT INTO Registration.NoteHeaderVoid
		(
			NoteHeaderID,
			NoteHeaderVoidReasonID,
			Comments,
			IsActive,
			ModifiedBy,
			ModifiedOn,
			CreatedBy,
			CreatedOn
		)
		SELECT
			@ID,
			NoteHeaderVoidReasonID,
			Comments,
			1,
			@ModifiedBy,
			@ModifiedOn,
			@ModifiedBy,
			@ModifiedOn
		FROM
			Registration.NoteHeaderVoid NHV
			INNER JOIN @IDstoCopy ITC
				ON NHV.NoteHeaderVoidID = ITC.NoteHeaderVoidID
		WHERE
			ITC.PKID = @PKID;

		SELECT @ID = SCOPE_IDENTITY();

		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'NoteHeaderVoid', @ID, NULL, @TransactionLogID, 45, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'NoteHeaderVoid', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		END

	INSERT INTO Registration.ContactDischargeNote
	(
		NoteHeaderID,
		ContactAdmissionID,
		DischargeReasonID,
		DischargeDate,
		SignatureStatusID,
		NoteText,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	SELECT
		@ID,
		ITC.ContactAdmissionID,
		DischargeReasonID,
		DischargeDate,
		SignatureStatusID,
		NoteText,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		Registration.ContactDischargeNote CDN
		INNER JOIN @IDstoCopy ITC
			ON CDN.ContactDischargeNoteID = ITC.PKID
	WHERE
		ITC.PKID = @PKID;

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'ContactDischargeNote', @ID, NULL, @TransactionLogID, 45, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'ContactDischargeNote', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1;
	UPDATE @IDstoCopy
	SET completed = 1
	WHERE
		PKID = @PKID;
	END

	--Copy Signature mapping records
	DECLARE @IDS TABLE (PrimaryKey BIGINT, ContactAdmissionID BIGINT, Completed BIT)

	INSERT INTO @IDS
	SELECT
		EDES.DocumentEntitySignatureID,
		D.NewContactAdmissionID,
		0
	FROM
		ESignature.DocumentEntitySignatures EDES
		INNER JOIN #Discharges D
			ON EDES.DocumentID = D.ContactAdmissionID
	WHERE
		DocumentTypeID = 8;

	SELECT @Loop = COUNT(*) FROM @IDS WHERE Completed = 0;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = PrimaryKey FROM @IDS WHERE Completed = 0;
	SET ROWCOUNT 0

	INSERT INTO ESignature.DocumentEntitySignatures
	(
		DocumentID,
		EntitySignatureID,
		DocumentTypeID,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	SELECT
		IDS.ContactAdmissionID,
		EDES.EntitySignatureID,
		EDES.DocumentTypeID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		ESignature.DocumentEntitySignatures EDES
		INNER JOIN @IDS IDS
			ON EDES.DocumentEntitySignatureID = IDS.PrimaryKey
	WHERE
		IDS.PrimaryKey = @PKID;

	SELECT @ID = SCOPE_IDENTITY();
			
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'ESignature', 'DocumentEntitySignatures', @ID, NULL, @TransactionLogID, 45, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'ESignature', 'DocumentEntitySignatures', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1;
	UPDATE @IDS
	SET Completed = 1
	WHERE
		PrimaryKey = @PKID;
	END

	DROP TABLE #ActiveAdmissions
	DROP TABLE #Discharges
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END