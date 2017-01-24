-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_MergeContactForms
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

CREATE PROCEDURE [Core].usp_MergeContactForms
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
			@ContactFormIDList NVARCHAR(MAX);

	-- Get the records from both the Parent and Child and create new records for the new merged Contact
	SELECT
		CF.ContactFormsID,
		CAST(0 AS BIGINT) AS NewContactFormsID,
		MAR.ResponseID,
		MAR.NewResponseID,
		0 AS Completed
	INTO
		#Forms
	FROM
		Registration.ContactForms CF
		LEFT OUTER JOIN Core.MergedAssessmentResponse MAR
			ON CF.ResponseID = MAR.ResponseID
	WHERE
		CF.ContactID IN (@ParentID, @ChildID);

	DECLARE @PKID BIGINT, @Loop INT

	SELECT @Loop = COUNT(*) FROM #Forms;
	SET @TotalRecords = @Loop;
	SET @TotalRecordsMerged = @Loop;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = ContactFormsID FROM #Forms WHERE Completed = 0;
	SET ROWCOUNT 0

	INSERT INTO Registration.ContactForms
	(
		ContactID,
		AssessmentID,
		ResponseID,
		UserID,
		DocumentStatusID,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	SELECT
		@ContactID,
		CF.AssessmentID,
		NewResponseID,
		CF.UserID,
		CF.DocumentStatusID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	FROM
		Registration.ContactForms CF
		INNER JOIN #Forms F
			ON CF.ContactFormsID = F.ContactFormsID
	WHERE
		F.ContactFormsID = @PKID;

	SELECT @ID = SCOPE_IDENTITY();
		
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'ContactForms', @ID, NULL, @TransactionLogID, 82, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'ContactForms', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @Loop = @Loop - 1;
	UPDATE #Forms
	SET Completed = 1,
		NewContactFormsID = @ID
	WHERE
		ContactFormsID = @PKID;
	END
	
	--Copy/Update services
	SELECT @ContactFormIDList = COALESCE(@ContactFormIDList + ',' , '') + CONVERT(NVARCHAR(MAX), ContactFormsID) FROM #Forms;

	EXEC Auditing.usp_AddServiceRecordingHistory @TransactionLogID, @ContactFormIDList, 5, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	DECLARE @IDstoUpdate TABLE (PKID bigint, ContactFormsID bigint, Completed tinyint DEFAULT(0));

	INSERT INTO @IDstoUpdate
	SELECT
		SR.ServiceRecordingID,
		F.NewContactFormsID,
		0
	FROM
		Core.ServiceRecording SR
		INNER JOIN #Forms F
			ON SR.SourceHeaderID = F.ContactFormsID
	WHERE
		SR.ServiceRecordingSourceID = 5;
	
	SELECT @Loop = COUNT(*) FROM @IDstoUpdate WHERE Completed = 0;

	WHILE @LOOP > 0

	BEGIN

	SET ROWCOUNT 1
	SELECT @PKID = PKID FROM @IDstoUpdate WHERE Completed = 0;
	SET ROWCOUNT 0
		
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'ServiceRecording', @PKID, NULL, @TransactionLogID, 72, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	UPDATE Core.ServiceRecording
	SET SourceHeaderID = ITU.ContactFormsID
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
	
	DROP TABLE #Forms
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END