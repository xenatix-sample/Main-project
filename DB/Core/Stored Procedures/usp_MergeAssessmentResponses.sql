-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_MergeAssessmentResponses
-- Author:		John Crossen
-- Date:		08/03/2016
--
-- Purpose:		Main procedure for Client AssessmentResponses for a specific document type
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

CREATE PROCEDURE [Core].usp_MergeAssessmentResponses
(
	@TransactionLogID BIGINT,
	@ContactID BIGINT,
	@ParentID BIGINT,
	@ChildID BIGINT,
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
			@EntitySignatureID BIGINT,
			@EntityTypeID INT;

	-- Get the records from both the Parent and Child and create new records for the new merged Contact
	SELECT
		AR.ResponseID,
		CAST(0 AS BIGINT) AS NewResponseID,
		0 AS Completed
	INTO #Responses
	FROM
		Core.AssessmentResponses AR
		INNER JOIN Core.DocumentMapping DM
			ON AR.AssessmentID = DM.AssessmentID
	WHERE
		AR.IsActive = 1
		AND AR.ContactID IN (@ParentID, @ChildID);

	DECLARE @PKID BIGINT, @Loop INT

	SELECT @Loop = COUNT(*) FROM #Responses;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = ResponseID FROM #Responses WHERE Completed = 0;
	SET ROWCOUNT 0

	INSERT INTO Core.AssessmentResponses
	(
		ContactID,
		AssessmentID,
		EnterDate,
		EnterDateINT,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	SELECT
		@ContactID,
		AssessmentID,
		EnterDate,
		EnterDateINT,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		Core.AssessmentResponses AR
		INNER JOIN #Responses R
			ON AR.ResponseID = R.ResponseID
	WHERE
		R.ResponseID = @PKID;

	SELECT @ID = SCOPE_IDENTITY();
		
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'AssessmentResponses', @ID, NULL, @TransactionLogID, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'AssessmentResponses', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1;
	UPDATE #Responses
	SET Completed = 1,
		NewResponseID = @ID
	WHERE
		ResponseID = @PKID;
	END
	
	--Copy response details to newly created responses
	SELECT
		ARD.ResponseDetailsID,
		NewResponseID AS ResponseID,
		DocumentEntitySignatureID,
		0 AS Completed
	INTO
		#Signatures
	FROM
		Core.AssessmentResponseDetails ARD
		INNER JOIN Core.AssessmentResponses AR
			ON ARD.ResponseID = AR.ResponseID
		INNER JOIN Core.DocumentMapping DM
			ON AR.AssessmentID = DM.AssessmentID
		INNER JOIN #Responses R
			ON ARD.ResponseID = R.ResponseID
		INNER JOIN ESignature.DocumentEntitySignatures EDES
			ON ARD.ResponseDetailsID = EDES.DocumentID
			AND DM.DocumentTypeID = EDES.DocumentTypeID
	WHERE
		ARD.IsActive = 1;

	SELECT @Loop = COUNT(*) FROM #Signatures;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = ResponseDetailsID FROM #Signatures WHERE Completed = 0;
	SET ROWCOUNT 0

	INSERT INTO Core.AssessmentResponseDetails
	(
		ResponseID,
		QuestionID,
		AssessmentSectionID,
		OptionsID,
		ResponseText,
		Rating,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	SELECT
		S.ResponseID,
		QuestionID,
		AssessmentSectionID,
		OptionsID,
		ResponseText,
		Rating,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		Core.AssessmentResponseDetails ARD
		INNER JOIN #Signatures S
			ON ARD.ResponseDetailsID = S.ResponseDetailsID
	WHERE
		S.ResponseDetailsID = @PKID;

	SELECT @ID = SCOPE_IDENTITY();

	SELECT
		@EntitySignatureID = EntitySignatureID,
		@EntityTypeID = EES.EntityTypeID
	FROM
		ESignature.EntitySignatures EES
	WHERE
		EES.EntitySignatureID IN (SELECT EntitySignatureID FROM ESignature.DocumentEntitySignatures EDES INNER JOIN #Signatures S ON EDES.DocumentEntitySignatureID = S.DocumentEntitySignatureID WHERE S.ResponseDetailsID = @PKID);

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
			EES.EntitySignatureID = @EntitySignatureID;

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
		@ID,
		EDES.EntitySignatureID,
		EDES.DocumentTypeID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		ESignature.DocumentEntitySignatures EDES
		INNER JOIN #Signatures S
			ON EDES.DocumentEntitySignatureID = S.DocumentEntitySignatureID
	WHERE
		S.ResponseDetailsID = @PKID;

	SELECT @ID = SCOPE_IDENTITY();
			
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'ESignature', 'DocumentEntitySignatures', @ID, NULL, @TransactionLogID, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'ESignature', 'DocumentEntitySignatures', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1;
	UPDATE #Signatures
	SET Completed = 1
	WHERE
		ResponseDetailsID = @PKID;
	END

	INSERT INTO Core.AssessmentResponseDetails
	(
		ResponseID,
		QuestionID,
		AssessmentSectionID,
		OptionsID,
		ResponseText,
		Rating,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	SELECT
		R.NewResponseID,
		QuestionID,
		AssessmentSectionID,
		OptionsID,
		ResponseText,
		Rating,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		Core.AssessmentResponseDetails ARD
		INNER JOIN Core.AssessmentResponses AR
			ON ARD.ResponseID = AR.ResponseID
		INNER JOIN Core.DocumentMapping DM
			ON AR.AssessmentID = DM.AssessmentID
		INNER JOIN #Responses R
			ON ARD.ResponseID = R.ResponseID
		LEFT OUTER JOIN ESignature.DocumentEntitySignatures EDES
			ON ARD.ResponseDetailsID = EDES.DocumentID
			AND DM.DocumentTypeID = EDES.DocumentTypeID
	WHERE
		EDES.DocumentEntitySignatureID IS NULL;

	INSERT INTO Core.MergedAssessmentResponse
	SELECT
		@TransactionLogID,
		ResponseID,
		NewResponseID
	FROM
		#Responses;

	DROP TABLE #Responses
	DROP TABLE #Signatures
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END