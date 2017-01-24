-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_MergeFinancialSummary
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
-- 01/09/2017	Scott Martin	Fixed an issue where signature data wasn't be copied from the orignal record
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].usp_MergeFinancialSummary
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
			@ID BIGINT,
			@EntityTypeID INT;

	-- Get the records from both the Parent and Child and create new records for the new merged Contact
	SELECT
		FS.FinancialSummaryID,
		CAST(0 AS BIGINT) AS NewFinancialSummaryID,
		0 AS Completed
	INTO #Summary
	FROM
		Registration.FinancialSummary FS
	WHERE
		FS.ContactID IN (@ParentID, @ChildID);

	DECLARE @PKID BIGINT, @Loop INT

	SELECT @Loop = COUNT(*) FROM #Summary;
	SET @TotalRecords = @Loop;
	SET @TotalRecordsMerged = @Loop;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = FinancialSummaryID FROM #Summary WHERE Completed = 0;
	SET ROWCOUNT 0

	INSERT INTO Registration.FinancialSummary
	(
		ContactID,
		OrganizationID,
		FinancialAssessmentXML,
		DateSigned,
		EffectiveDate,
		AssessmentEndDate,
		ExpirationDate,
		SignatureStatusID,
		UserID,
		UserPhoneID,
		CredentialID,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	SELECT
		@ContactID,
		OrganizationID,
		FinancialAssessmentXML,
		DateSigned,
		EffectiveDate,
		AssessmentEndDate,
		ExpirationDate,
		SignatureStatusID,
		UserID,
		UserPhoneID,
		CredentialID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		Registration.FinancialSummary FS
		INNER JOIN #Summary S
			ON FS.FinancialSummaryID = S.FinancialSummaryID
	WHERE
		S.FinancialSummaryID = @PKID;

	SELECT @ID = SCOPE_IDENTITY();
		
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'FinancialSummary', @PKID, NULL, @TransactionLogID, 71, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'FinancialSummary', @AuditDetailID, @PKID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1;
	UPDATE #Summary
	SET Completed = 1,
		NewFinancialSummaryID = @ID
	WHERE
		FinancialSummaryID = @PKID;
	END

	--Copy confirmation statement records
	SELECT
		FSCS.FinancialSummaryConfirmationStatementID,
		CAST(0 AS BIGINT) AS NewFinancialSummaryConfirmationStatementID,
		S.NewFinancialSummaryID,
		0 AS Completed
	INTO
		#Statements
	FROM
		Registration.FinancialSummaryConfirmationStatement FSCS
		INNER JOIN #Summary S
			ON FSCS.FinancialSummaryID = S.FinancialSummaryID;

	SELECT @Loop = COUNT(*) FROM #Statements WHERE Completed = 0;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = FinancialSummaryConfirmationStatementID FROM #Statements WHERE Completed = 0;
	SET ROWCOUNT 0

	INSERT INTO Registration.FinancialSummaryConfirmationStatement
	(
		FinancialSummaryID,
		ConfirmationStatementID,
		DateSigned,
		SignatureStatusID,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	SELECT
		S.NewFinancialSummaryID,
		FSCS.ConfirmationStatementID,
		FSCS.DateSigned,
		FSCS.SignatureStatusID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		Registration.FinancialSummaryConfirmationStatement FSCS
		INNER JOIN #Statements S
			ON FSCS.FinancialSummaryConfirmationStatementID = S.FinancialSummaryConfirmationStatementID
	WHERE
		S.FinancialSummaryConfirmationStatementID = @PKID;

	SELECT @ID = SCOPE_IDENTITY();
		
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'FinancialSummaryConfirmationStatement', @ID, NULL, @TransactionLogID, 71, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'FinancialSummaryConfirmationStatement', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop-1
	
	UPDATE #Statements
	SET Completed = 1,
		NewFinancialSummaryConfirmationStatementID = @ID
	WHERE
		FinancialSummaryConfirmationStatementID = @PKID;
	END

	--Copy Signature mapping records
	DECLARE @IDS TABLE (PrimaryKey BIGINT, DocumentID BIGINT, Completed BIT)

	INSERT INTO @IDS
	SELECT
		EDES.DocumentEntitySignatureID,
		 S.NewFinancialSummaryID,
		0
	FROM
		ESignature.DocumentEntitySignatures EDES
		INNER JOIN #Summary S
			ON EDES.DocumentID = S.FinancialSummaryID
	WHERE
		DocumentTypeID = 11;

	SELECT @Loop = COUNT(*) FROM @IDS WHERE Completed = 0;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = PrimaryKey FROM @IDS WHERE Completed = 0;
	SET ROWCOUNT 0

	SELECT
		@ID = EntitySignatureID,
		@EntityTypeID = EES.EntityTypeID
	FROM
		ESignature.EntitySignatures EES
	WHERE
		EES.EntitySignatureID IN (SELECT EntitySignatureID FROM ESignature.DocumentEntitySignatures WHERE DocumentEntitySignatureID = @PKID);

	IF @EntityTypeID = 2
		BEGIN
		INSERT INTO ESignature.EntitySignatures
		(
			EntityID,
			EntityName,
			SignatureID,
			EntityTypeID,
			SignatureTypeID,
			CredentialID,
			IsActive,
			ModifiedBy,
			ModifiedOn,
			CreatedBy,
			CreatedOn
		)
		SELECT
			@ContactID,
			EntityName,
			SignatureID,
			EntityTypeID,
			SignatureTypeID,
			CredentialID,
			IsActive,
			ModifiedBy,
			ModifiedOn,
			CreatedBy,
			CreatedOn
		FROM
			ESignature.EntitySignatures EES
		WHERE
			EES.EntitySignatureID = @ID;

		SELECT @ID = SCOPE_IDENTITY();

		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'ESignature', 'EntitySignatures', @ID, NULL, @TransactionLogID, 71, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'ESignature', 'EntitySignatures', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		END

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
		IDS.DocumentID,
		@ID,
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
			
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'ESignature', 'DocumentEntitySignatures', @ID, NULL, @TransactionLogID, 71, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'ESignature', 'DocumentEntitySignatures', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1;
	UPDATE @IDS
	SET Completed = 1
	WHERE
		PrimaryKey = @PKID;
	END

	DELETE FROM @IDS;

	INSERT INTO @IDS
	SELECT
		EDES.DocumentEntitySignatureID,
		 S.NewFinancialSummaryConfirmationStatementID,
		0
	FROM
		ESignature.DocumentEntitySignatures EDES
		INNER JOIN #Statements S
			ON EDES.DocumentID = S.FinancialSummaryConfirmationStatementID
	WHERE
		DocumentTypeID = 12;

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
		IDS.DocumentID,
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
			
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'ESignature', 'DocumentEntitySignatures', @ID, NULL, @TransactionLogID, 71, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'ESignature', 'DocumentEntitySignatures', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1;
	UPDATE @IDS
	SET Completed = 1
	WHERE
		PrimaryKey = @PKID;
	END

	DROP TABLE #Summary
	DROP TABLE #Statements
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END