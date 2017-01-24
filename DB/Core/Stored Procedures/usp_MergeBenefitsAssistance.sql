-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_MergeBenefitsAssistance
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
-- 01/19/2017	Scott Martin	Fixed an issue where only Benefits Assistance records with an assessment response were being merged
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].usp_MergeBenefitsAssistance
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
			@BenefitsAssistanceIDList NVARCHAR(MAX);

	-- Get the records from both the Parent and Child and create new records for the new merged Contact
	

	SELECT
		BA.BenefitsAssistanceID,
		CAST(0 AS BIGINT) AS NewBenefitsAssistanceID,
		MAR.ResponseID,
		MAR.NewResponseID,
		0 AS Completed
	INTO
		#BA
	FROM
		Registration.BenefitsAssistance BA
		LEFT OUTER JOIN Core.MergedAssessmentResponse MAR
			ON BA.ResponseID = MAR.ResponseID
	WHERE
		BA.ContactID IN (@ParentID, @ChildID);

	DECLARE @PKID BIGINT, @Loop INT

	SELECT @Loop = COUNT(*) FROM #BA;
	SET @TotalRecords = @Loop;
	SET @TotalRecordsMerged = @Loop;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = BenefitsAssistanceID FROm #BA WHERE Completed = 0;
	SET ROWCOUNT 0

	INSERT INTO Registration.BenefitsAssistance
	(
		ContactID,
		DateEntered,
		UserID,
		AssessmentID,
		ResponseID,
		DocumentStatusID,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	SELECT
		@ContactID,
		BA.DateEntered,
		BA.UserID,
		BA.AssessmentID,
		B.NewResponseID,
		BA.DocumentStatusID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		Registration.BenefitsAssistance BA
		INNER JOIN #BA B
			ON BA.BenefitsAssistanceID = B.BenefitsAssistanceID
	WHERE
		B.BenefitsAssistanceID = @PKID;

	SELECT @ID = SCOPE_IDENTITY();
		
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'BenefitsAssistance', @ID, NULL, @TransactionLogID, 72, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'BenefitsAssistance', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1;
	UPDATE #BA
	SET Completed = 1,
		NewBenefitsAssistanceID = @ID
	WHERE
		BenefitsAssistanceID = @PKID;
	END

	--Copy/Update services
	SELECT @BenefitsAssistanceIDList = COALESCE(@BenefitsAssistanceIDList + ',' , '') + CONVERT(NVARCHAR(MAX), BenefitsAssistanceID) FROM #BA;

	EXEC Auditing.usp_AddServiceRecordingHistory @TransactionLogID, @BenefitsAssistanceIDList, 4, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	DECLARE @IDstoUpdate TABLE (PKID bigint, BenefitsAssistanceID bigint, Completed tinyint DEFAULT(0));

	INSERT INTO @IDstoUpdate
	SELECT
		SR.ServiceRecordingID,
		BA.NewBenefitsAssistanceID,
		0
	FROM
		Core.ServiceRecording SR
		INNER JOIN #BA BA
			ON SR.SourceHeaderID = BA.BenefitsAssistanceID
	WHERE
		SR.ServiceRecordingSourceID = 4;
	
	SELECT @Loop = COUNT(*) FROM @IDstoUpdate WHERE Completed = 0;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = PKID FROM @IDstoUpdate WHERE Completed = 0;
	SET ROWCOUNT 0
		
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'ServiceRecording', @PKID, NULL, @TransactionLogID, 72, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	UPDATE Core.ServiceRecording
	SET SourceHeaderID = ITU.BenefitsAssistanceID
	FROM
		Core.ServiceRecording SR
		INNER JOIN @IDstoUpdate ITU
			ON SR.ServiceRecordingID = ITU.PKID
	WHERE
		ITU.PKID = @PKID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'ServiceRecording', @AuditDetailID, @PKID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop-1
	
	UPDATE @IDstoUpdate
	SET Completed = 1
	WHERE
		PKID = @PKID;
	END
	
	DROP TABLE #BA;
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END