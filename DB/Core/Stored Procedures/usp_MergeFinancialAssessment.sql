-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_MergeFinancialAssessment
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

CREATE PROCEDURE [Core].usp_MergeFinancialAssessment
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
		FA.FinancialAssessmentID,
		CAST(0 AS BIGINT) AS NewFinancialAssessmentID,
		0 AS Completed
	INTO
		#Income
	FROM
		Registration.FinancialAssessment FA
	WHERE
		FA.ContactID IN (@ParentID, @ChildID)
		AND FA.IsActive = 1;

	DECLARE @PKID BIGINT, @Loop INT

	SELECT @Loop = COUNT(*) FROM #Income;
	SET @TotalRecords = @Loop;
	SET @TotalRecordsMerged = @Loop;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = FinancialAssessmentID FROM #Income WHERE Completed = 0;
	SET ROWCOUNT 0

	INSERT INTO Registration.FinancialAssessment
	(
		ContactID,
		AssessmentDate,
		TotalIncome,
		TotalExpenses,
		TotalExtraOrdinaryExpenses,
		TotalOther,
		AdjustedGrossIncome,
		FamilySize,
		ExpirationDate,
		ExpirationReasonID,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	SELECT
		@ContactID,
		AssessmentDate,
		TotalIncome,
		TotalExpenses,
		TotalExtraOrdinaryExpenses,
		TotalOther,
		AdjustedGrossIncome,
		FamilySize,
		ExpirationDate,
		ExpirationReasonID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		Registration.FinancialAssessment FA
		INNER JOIN #Income I
			ON FA.FinancialAssessmentID = I.FinancialAssessmentID
	WHERE
		I.FinancialAssessmentID = @PKID;

	SELECT @ID = SCOPE_IDENTITY();
		
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'FinancialAssessment', @ID, NULL, @TransactionLogID, 34, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'FinancialAssessment', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1;

	UPDATE #Income
	SET Completed = 1,
		NewFinancialAssessmentID = @ID
	WHERE
		FinancialAssessmentID = @PKID;
	END

	--Copy detail records
	DECLARE @NewFinancialAssessmentID BIGINT;

	DECLARE @IDstoCopy TABLE
	(
		PKID bigint,
		FinancialAssessmentID bigint,
		completed tinyint DEFAULT(0)
	);

	INSERT INTO @IDstoCopy
	SELECT
		FAD.FinancialAssessmentDetailsID,
		I.NewFinancialAssessmentID,
		0
	FROM
		Registration.FinancialAssessmentDetails FAD
		INNER JOIN #Income I
			ON FAD.FinancialAssessmentID = I.FinancialAssessmentID
	WHERE
		I.NewFinancialAssessmentID > 0;

	SELECT @Loop = COUNT(*) FROM @IDstoCopy WHERE completed = 0;
	SET @TotalRecords = @TotalRecords + @Loop;
	SET @TotalRecordsMerged = @TotalRecordsMerged + @Loop;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT
		@PKID = PKID,
		@NewFinancialAssessmentID = FinancialAssessmentID
		FROM @IDstoCopy WHERE completed = 0;
	SET ROWCOUNT 0

	INSERT INTO Registration.FinancialAssessmentDetails
	(
		FinancialAssessmentID,
		CategoryTypeID,
		Amount,
		FinanceFrequencyID,
		CategoryID,
		RelationshipTypeID,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	SELECT
		@NewFinancialAssessmentID,
		CategoryTypeID,
		Amount,
		FinanceFrequencyID,
		CategoryID,
		RelationshipTypeID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		Registration.FinancialAssessmentDetails FAD
		INNER JOIN @IDstoCopy ITC
			ON FAD.FinancialAssessmentDetailsID = ITC.PKID
	WHERE
		ITC.PKID = @PKID;

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'FinancialAssessmentDetails', @ID, NULL, @TransactionLogID, 34, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'FinancialAssessmentDetails', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1;
	UPDATE @IDstoCopy
	SET completed = 1
	WHERE
		PKID = @PKID;
	END

	DROP TABLE #Income
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END