-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_SaveAssessmentResponseDetails]
-- Author:		Demetrios C. Christopher
-- Date:		10/12/2015
--
-- Purpose:		Save Assessment Response details (covers both Adds and Updates as they could be part of the same transaction)
--
-- Notes:		
--				
-- Depends:		 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/12/2015	Demetrios C. Christopher - Initial - Cloned from Core.usp_UpdateAssessmentResponseDetails
-- 11/05/2015	Demetrios C. Christopher - Ensured that any existing response detail data being deleted is limited to the same response/section
-- 11/22/2015	Rajiv Ranjan			 - Added modified by and modified on
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added SystemModifiedOn field
-- 04/16/2016	Scott Martin	Added code for saving signatures
-- 04/26/2006	Gurpreet Singh	Remove AuditXML and evaluated if need to save snapshot within sproc
-- 05/11/2016	Scott Martin	Fixed a bug where signatures were not being assigned to the correct response detail id
-- 07/26/2016	Gurpreet Singh	Remove call to Add/Update contact consent from with in sproc
-- 07/29/2016	Vishal Yadav	Response Text limit extended to max
-- 09/01/2016	Gurpreet Singh	Added ResponseID, SectionID and ModifiedOn
-- 11/09/2016	Atul Chauhan	Made changes to get CredentialID
-- 12/15/2016	Scott Martin	Added auditing to track when a change is made
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_SaveAssessmentResponseDetails]
	@AssessmentXML xml,
	@ResponseID bigint,
	@SectionID bigint,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);
				
	SELECT	@ResultCode = 0,
			@ResultMessage = 'executed successfully'

	BEGIN TRY
	-- We MUST pass in the AssessmentSectionID and ResponseID
	DECLARE @ContactID BIGINT,
			@DocumentID BIGINT,
			@DocumentTypeID INT,
			@EntityID BIGINT,
			@EntityTypeID INT,
			@SignatureBLOB VARBINARY(MAX),
			@SignatureID BIGINT,
			@InputTypeID INT,
			@DateSigned Date,
			@CredentialID int=null

	--DECLARE @useSnapshot bit	
	--select @useSnapshot = h.UseSnapshot from Core.Assessments h inner join
	-- Core.AssessmentSections s on h.AssessmentID=s.AssessmentID
	-- where s.AssessmentSectionID = @SectionID

	SELECT
		@ContactID = AR.ContactID,
		@DocumentTypeID = DM.DocumentTypeID
	FROM
		Core.AssessmentResponses AR
		INNER JOIN Core.DocumentMapping DM
			ON AR.AssessmentID = DM.AssessmentID
	WHERE
		AR.ResponseID = @ResponseID;

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'AssessmentResponseDetails', NULL, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'AssessmentResponseDetails', @AuditDetailID, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT
		T.C.value('ResponseDetailsID[1]','Bigint') as ResponseDetailsID,
		@ResponseID as ResponseID,
		T.C.value('QuestionID[1]','Bigint') as QuestionID,
		aq.AssessmentSectionID as AssessmentSectionID,
		T.C.value('(./OptionsID/text())[1]','Bigint') as OptionsID,
		T.C.value('ResponseText[1]','nvarchar(MAX)') as ResponseText,
		T.C.value('SignatureBLOB[1]','VARBINARY(MAX)') as SignatureBLOB,
		T.C.value('DateSigned[1]','Date') as DateSigned,
		T.C.value('CredentialID[1]','Int') as CredentialID,
		T.C.value('Rating[1]','Int') as Rating,
		@ModifiedOn as ModifiedOn
	INTO #Details
	FROM
		@AssessmentXML.nodes('Assessment/AssessmentResponseDetails') AS T(C)
		INNER JOIN [Core].[AssessmentQuestions] aq
			ON aq.QuestionID = T.C.value('QuestionID[1]','Bigint');

	DECLARE @Responses TABLE (MergeAction NVARCHAR(25), ResponseDetailsID BIGINT, QuestionID BIGINT);

	MERGE [Core].[AssessmentResponseDetails] AS target
	USING (SELECT ResponseDetailsID, ResponseID, QuestionID, AssessmentSectionID, OptionsID, ResponseText, Rating, ModifiedOn FROM #Details) AS source (ResponseDetailsID, ResponseID, QuestionID, AssessmentSectionID, OptionsID, ResponseText,  Rating, ModifiedOn)
	ON target.ResponseDetailsID = source.ResponseDetailsID AND target.ResponseID = source.ResponseID AND target.AssessmentSectionID = source.AssessmentSectionID
	WHEN NOT MATCHED BY TARGET THEN
		INSERT (ResponseID, QuestionID, AssessmentSectionID, OptionsID, ResponseText, Rating, IsActive, ModifiedBy, ModifiedOn, CreatedBy, CreatedOn)
		VALUES (source.ResponseID, source.QuestionID, source.AssessmentSectionID, source.OptionsID, source.ResponseText, source.Rating, 1, @ModifiedBy, source.ModifiedOn, @ModifiedBy, source.ModifiedOn)
	WHEN MATCHED THEN
		UPDATE SET
			OptionsID = source.OptionsID,
			ResponseText = source.ResponseText,
			Rating = source.Rating,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = source.ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
	WHEN NOT MATCHED BY SOURCE AND target.ResponseID = @ResponseID AND target.AssessmentSectionID = @SectionID THEN
		UPDATE SET
			IsActive = 0,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = GETUTCDATE(),
			SystemModifiedOn = GETUTCDATE()
	OUTPUT
		$action,
		inserted.ResponseDetailsID,
		inserted.QuestionID
	INTO
		@Responses;

	UPDATE #Details
	SET ResponseDetailsID = R.ResponseDetailsID
	FROM
		#Details D
		INNER JOIN (SELECT QuestionID, MAX(ResponseDetailsID) AS ResponseDetailsID FROM @Responses WHERE MergeAction <> 'Delete' GROUP BY QuestionID) AS R
			ON D.QuestionID = R.QuestionID
	WHERE
		ISNULL(D.ResponseDetailsID, 0) = 0;  
	
	DECLARE @SignCursor CURSOR;

	SET @SignCursor = CURSOR FOR
	SELECT
		D.ResponseDetailsID AS DocumentID,
		CASE
			WHEN AO.Options = 'User' THEN @ModifiedBy
			WHEN AO.Options = 'Contact' THEN @ContactID
			ELSE 0 END AS EntityID,
		ET.EntityTypeID,
		D.SignatureBLOB,
		AIT.InputTypeID,
		D.DateSigned,
		D.CredentialID
	FROM
		#Details D
		INNER JOIN Core.AssessmentQuestions AQ
			ON D.QuestionID = AQ.QuestionID
		INNER JOIN Reference.AssessmentInputType AIT
			ON AQ.InputTypeID = AIT.InputTypeID
		INNER JOIN Core.AssessmentOptions AO
			ON D.OptionsID = AO.OptionsID
		LEFT OUTER JOIN Reference.EntityType ET
			ON AO.Options = ET.EntityType
		LEFT OUTER JOIN ESignature.DocumentEntitySignatures DESS
			ON D.ResponseDetailsID = DESS.DocumentID
			AND DESS.DocumentTypeID = @DocumentTypeID
	WHERE
		(AIT.InputTypeID=11 OR AIT.InputTypeID=12)
		AND DESS.DocumentEntitySignatureID IS NULL 

	OPEN @SignCursor 
	FETCH NEXT FROM @SignCursor 
	INTO  @DocumentID, @EntityID, @EntityTypeID, @SignatureBLOB,@InputTypeID,@DateSigned,@CredentialID


	WHILE @@FETCH_STATUS = 0
		BEGIN
		if(@SignatureBLOB IS NOT NULL)
			BEGIN
				EXEC ESignature.usp_AddSignatures @SignatureBLOB, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @SignatureID OUTPUT;

			END
		ELSE
			BEGIN
				SET @ModifiedOn=@DateSigned
			END
		EXEC ESignature.usp_SaveDocumentEntitySignature @DocumentID, @DocumentTypeID, NULL, @EntityID, NULL, @EntityTypeID, @SignatureID, @CredentialID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT

		FETCH NEXT FROM @SignCursor
		INTO  @DocumentID, @EntityID, @EntityTypeID, @SignatureBLOB,@InputTypeID,@DateSigned,@CredentialID
		END; 

	CLOSE @SignCursor;
	DEALLOCATE @SignCursor;
	
	END TRY
	BEGIN CATCH
		SELECT 
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
