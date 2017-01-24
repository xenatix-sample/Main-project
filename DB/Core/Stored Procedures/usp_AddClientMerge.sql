-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_MergeClient
-- Author:		John Crossen
-- Date:		08/03/2016
--
-- Purpose:		Main procedure for Client Merging
--
-- Notes:		Default to using ComponentTypeID 1 (Screen) first and then 2 (Tile)
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/03/2016 - Initial procedure creation
-- 08/16/2016	Scott Martin	Refactored proc to include auditing and storing results of merging
-- 09/06/2016	Scott Martin	Updated result message/Added result message if ParentID/ChildID is NULL
-- 09/08/2016	Scott Martin	Added clauses to the ContactID lookup to only get it for Active contacts
-- 09/22/2016	Scott Martin	Added proc for signatures
-- 10/14/2016	Scott Martin	Added proc for client identifiers
-- 12/01/2016	Scott Martin	Refactored the client merge process
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_AddClientMerge](
	@ParentMRN BIGINT,
	@ChildMRN BIGINT,
	@MergeDate DATETIME,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID BIGINT OUTPUT
)
AS
BEGIN
	SELECT @ResultCode = 0,
	
	@ResultMessage = 'Data saved successfully',
	@ID = 0

	BEGIN TRY	
	-- Change MRNs to ContactIDs
	DECLARE @ContactID BIGINT,
			@ParentID BIGINT,
			@ChildID BIGINT,
			@AuditDetailID BIGINT,
			@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
			@TotalRecords INT = 0,
			@TotalRecordsMerged INT = 0,
			@IsSuccessful BIT,
			@TransactionLogID BIGINT;

	SELECT @ParentID = ContactID FROM Registration.ContactMRN WHERE MRN = @ParentMRN AND IsActive = 1;
	SELECT @ChildID = ContactID FROM Registration.ContactMRN WHERE MRN = @ChildMRN AND IsActive = 1;

	IF @ParentID IS NULL OR @ChildID IS NULL
		BEGIN
		SELECT @ResultCode = 16,
				@ResultMessage = 'Merge could not be completed. Please try again.'
		RETURN
		END

	CREATE TABLE #TLog (TransactionLogID NVARCHAR(16));

	INSERT INTO #TLog EXEC Core.usp_GenerateTransactionLogID @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	SELECT @TransactionLogID = TransactionLogID FROM #TLog;

	DROP TABLE #TLog;

	-- Create new contact to merge Parent/Child into
	EXEC Core.usp_MergeClientContact @TransactionLogID, @ParentID, @ChildID, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @ContactID OUTPUT;

	-- Insert into merge log table
	INSERT INTO Core.MergedContactsMapping
	(
		TransactionLogID,
		ContactID,
		ParentID,
		ChildID,
		MergeDate,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	VALUES
	(
		@TransactionLogID,
		@ContactID,
		@ParentID,
		@ChildID,
		@MergeDate,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'MergedContactsMapping', @ID, NULL, @TransactionLogID, 103, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'MergedContactsMapping', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Core.usp_AddContactMergeResult @ID, 23, 6, 5, @ResultCode, @ResultMessage;

	EXEC Core.usp_MergeAssessmentResponses @TransactionLogID, @ContactID, @ParentID, @ChildID, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	---------------------------------------------------------------------------Registration------------------------------------------------------------------------------
	-- Contact Client Identifier 23
	EXEC Core.usp_MergeContactClientIdentifiers @TransactionLogID, @ContactID, @ParentID, @ChildID, @TotalRecords OUTPUT, @TotalRecordsMerged OUTPUT, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	EXEC Core.usp_AddContactMergeResult @ID, 23, @TotalRecords, @TotalRecordsMerged, @ResultCode, @ResultMessage;

	-- Contact Client Alias 23
	EXEC Core.usp_MergeContactAlias @TransactionLogID, @ContactID, @ParentID, @ChildID, @TotalRecords OUTPUT, @TotalRecordsMerged OUTPUT, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	EXEC Core.usp_AddContactMergeResult @ID, 23, @TotalRecords, @TotalRecordsMerged, @ResultCode, @ResultMessage;

	-- Contact Race 24
	EXEC Core.usp_MergeContactRace @TransactionLogID, @ContactID, @ParentID, @ChildID, @TotalRecords OUTPUT, @TotalRecordsMerged OUTPUT, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	EXEC Core.usp_AddContactMergeResult @ID, 24, @TotalRecords, @TotalRecordsMerged, @ResultCode, @ResultMessage;

	-- Contact Relationship 26
	EXEC Core.usp_MergeContactRelationShip @TransactionLogID, @ContactID, @ParentID, @ChildID, @TotalRecords OUTPUT, @TotalRecordsMerged OUTPUT, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	EXEC Core.usp_AddContactMergeResult @ID, 26, @TotalRecords, @TotalRecordsMerged, @ResultCode, @ResultMessage;

	-- Contact Payor 33
	EXEC Core.usp_MergeContactPayor @TransactionLogID, @ContactID, @ParentID, @ChildID, @TotalRecords OUTPUT, @TotalRecordsMerged OUTPUT, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	EXEC Core.usp_AddContactMergeResult @ID, 33, @TotalRecords, @TotalRecordsMerged, @ResultCode, @ResultMessage;

	-- Financial Assessment 34
	EXEC Core.usp_MergeFinancialAssessment @TransactionLogID, @ContactID,  @ParentID, @ChildID, @TotalRecords OUTPUT, @TotalRecordsMerged OUTPUT, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	EXEC Core.usp_AddContactMergeResult @ID, 34, @TotalRecords, @TotalRecordsMerged, @ResultCode, @ResultMessage;


	---------------------------------------------------------------------------General------------------------------------------------------------------------------
	-- Merge Contact Address 48
	EXEC Core.usp_MergeClientContactAddress @TransactionLogID, @ContactID, @ParentID, @ChildID, @TotalRecords OUTPUT, @TotalRecordsMerged OUTPUT, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	EXEC Core.usp_AddContactMergeResult @ID, 48, @TotalRecords, @TotalRecordsMerged, @ResultCode, @ResultMessage;

	-- Merge Contact Phone 49
	EXEC Core.usp_MergeClientContactPhone @TransactionLogID, @ContactID, @ParentID, @ChildID, @TotalRecords OUTPUT, @TotalRecordsMerged OUTPUT, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	EXEC Core.usp_AddContactMergeResult @ID, 49, @TotalRecords, @TotalRecordsMerged, @ResultCode, @ResultMessage;
	
	-- Merge Contact Email 50
	EXEC Core.usp_MergeClientContactEmail @TransactionLogID, @ContactID, @ParentID, @ChildID, @TotalRecords OUTPUT, @TotalRecordsMerged OUTPUT, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	EXEC Core.usp_AddContactMergeResult @ID, 50, @TotalRecords, @TotalRecordsMerged, @ResultCode, @ResultMessage;

	-- Contact Admission 45
	EXEC Core.usp_MergeContactAdmission @TransactionLogID, @ContactID, @ParentID, @ChildID, @TotalRecords OUTPUT, @TotalRecordsMerged OUTPUT, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	EXEC Core.usp_AddContactMergeResult @ID, 45, @TotalRecords, @TotalRecordsMerged, @ResultCode, @ResultMessage;

	-- Contact Referral 42
	EXEC Core.usp_MergeContactReferral @TransactionLogID, @ContactID, @ParentID, @ChildID, @TotalRecords OUTPUT, @TotalRecordsMerged OUTPUT, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	EXEC Core.usp_AddContactMergeResult @ID, 42, @TotalRecords, @TotalRecordsMerged, @ResultCode, @ResultMessage;


	---------------------------------------------------------------------------Benefits------------------------------------------------------------------------------
	-- Financial Sumamry 71
	EXEC Core.usp_MergeFinancialSummary @TransactionLogID, @ContactID, @ParentID, @ChildID, @TotalRecords OUTPUT, @TotalRecordsMerged OUTPUT, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	EXEC Core.usp_AddContactMergeResult @ID, 71, @TotalRecords, @TotalRecordsMerged, @ResultCode, @ResultMessage;
		
	-- Benefits Assistance 72
	EXEC Core.usp_MergeBenefitsAssistance @TransactionLogID, @ContactID, @ParentID, @ChildID, @TotalRecords OUTPUT, @TotalRecordsMerged OUTPUT, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	EXEC Core.usp_AddContactMergeResult @ID, 72, @TotalRecords, @TotalRecordsMerged, @ResultCode, @ResultMessage;

	-- SelfPay 70
	EXEC Core.usp_MergeSelfPay @TransactionLogID, @ContactID, @ParentID, @ChildID, @TotalRecords OUTPUT, @TotalRecordsMerged OUTPUT, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	EXEC Core.usp_AddContactMergeResult @ID, 70, @TotalRecords, @TotalRecordsMerged, @ResultCode, @ResultMessage;


	---------------------------------------------------------------------------Call Center------------------------------------------------------------------------------
	-- CallCenterHeader (Crisis Line) 43
	EXEC Core.usp_MergeCallCenterHeader @TransactionLogID, @ContactID, @ParentID, @ChildID, 1, @TotalRecords OUTPUT, @TotalRecordsMerged OUTPUT, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	EXEC Core.usp_AddContactMergeResult @ID, 43, @TotalRecords, @TotalRecordsMerged, @ResultCode, @ResultMessage;

	-- CallCenterHeader (Law Liaison) 44
	EXEC Core.usp_MergeCallCenterHeader @TransactionLogID, @ContactID, @ParentID, @ChildID, 2, @TotalRecords OUTPUT, @TotalRecordsMerged OUTPUT, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	EXEC Core.usp_AddContactMergeResult @ID, 44, @TotalRecords, @TotalRecordsMerged, @ResultCode, @ResultMessage;


	---------------------------------------------------------------------------Clinical------------------------------------------------------------------------------
	---- Chief Complaint 54
	--EXEC Core.usp_MergeChiefComplaint @ID, @ParentID, @ChildID, @TotalRecords OUTPUT, @TotalRecordsMerged OUTPUT, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	--EXEC Core.usp_AddContactMergeResult @ID, 54, @TotalRecords, @TotalRecordsMerged, @ResultCode, @ResultMessage;

	---- Clinical Assessments 60
	--EXEC Core.usp_MergeClinicalAssessments @ID, @ParentID, @ChildID, @TotalRecords OUTPUT, @TotalRecordsMerged OUTPUT, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	--EXEC Core.usp_AddContactMergeResult @ID, 60, @TotalRecords, @TotalRecordsMerged, @ResultCode, @ResultMessage;
		
	---- Contact Allergy 53
	--EXEC Core.usp_MergeContactAllergy @ID, @ParentID, @ChildID, @TotalRecords OUTPUT, @TotalRecordsMerged OUTPUT, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	--EXEC Core.usp_AddContactMergeResult @ID, 53, @TotalRecords, @TotalRecordsMerged, @ResultCode, @ResultMessage;

	---- Encounter 
	----EXEC Core.usp_MergeEncounter @ID, @ParentID, @ChildID, @TotalRecords OUTPUT, @TotalRecordsMerged OUTPUT, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	----EXEC Core.usp_AddContactMergeResult @ID, NULL, @TotalRecords, @TotalRecordsMerged, @ResultCode, @ResultMessage;

	---- Clinical Family Relationship 64
	--EXEC Core.usp_MergeClinicalFamilyRelationship @ID, @ParentID, @ChildID, @TotalRecords OUTPUT, @TotalRecordsMerged OUTPUT, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	--EXEC Core.usp_AddContactMergeResult @ID, 64, @TotalRecords, @TotalRecordsMerged, @ResultCode, @ResultMessage;

	---- HPI 66
	--EXEC Core.usp_MergeHPI @ID, @ParentID, @ChildID, @TotalRecords OUTPUT, @TotalRecordsMerged OUTPUT, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	--EXEC Core.usp_AddContactMergeResult @ID, 66, @TotalRecords, @TotalRecordsMerged, @ResultCode, @ResultMessage;
	
	---- medical History 
	----EXEC Core.usp_MergeMedicalHistory @ID, @ParentID, @ChildID, @TotalRecords OUTPUT, @TotalRecordsMerged OUTPUT, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	----EXEC Core.usp_AddContactMergeResult @ID, NULL, @TotalRecords, @TotalRecordsMerged, @ResultCode, @ResultMessage;

	---- Clinical Notes 55
	--EXEC Core.usp_MergeClinicalNotes @ID, @ParentID, @ChildID, @TotalRecords OUTPUT, @TotalRecordsMerged OUTPUT, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	--EXEC Core.usp_AddContactMergeResult @ID, 55, @TotalRecords, @TotalRecordsMerged, @ResultCode, @ResultMessage;
		
	---- Review of Systems 59
	--EXEC Core.usp_MergeReviewofSystems @ID, @ParentID, @ChildID, @TotalRecords OUTPUT, @TotalRecordsMerged OUTPUT, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	--EXEC Core.usp_AddContactMergeResult @ID, 59, @TotalRecords, @TotalRecordsMerged, @ResultCode, @ResultMessage;

	---- Social Relationship 64
	--EXEC Core.usp_MergeSocialRelationship @ID, @ParentID, @ChildID, @TotalRecords OUTPUT, @TotalRecordsMerged OUTPUT, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	--EXEC Core.usp_AddContactMergeResult @ID, 64, @TotalRecords, @TotalRecordsMerged, @ResultCode, @ResultMessage;

	---- Surgical History 
	----EXEC Core.usp_MergeSurgicalHistory @ID, @ParentID, @ChildID, @TotalRecords OUTPUT, @TotalRecordsMerged OUTPUT, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	----EXEC Core.usp_AddContactMergeResult @ID, NULL, @TotalRecords, @TotalRecordsMerged, @ResultCode, @ResultMessage;

	---- Vitals 65
	--EXEC Core.usp_MergeVitals @ID, @ParentID, @ChildID, @TotalRecords OUTPUT, @TotalRecordsMerged OUTPUT, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	--EXEC Core.usp_AddContactMergeResult @ID, 65, @TotalRecords, @TotalRecordsMerged, @ResultCode, @ResultMessage;


	---------------------------------------------------------------------------ECI------------------------------------------------------------------------------
	---- Eligibility 36
	--EXEC Core.usp_MergeEligibility @ID, @ParentID, @ChildID, @TotalRecords OUTPUT, @TotalRecordsMerged OUTPUT, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	--EXEC Core.usp_AddContactMergeResult @ID, 36, @TotalRecords, @TotalRecordsMerged, @ResultCode, @ResultMessage;

	---- IFSP 37
	--EXEC Core.usp_MergeIFSP @ID, @ParentID, @ChildID, @TotalRecords OUTPUT, @TotalRecordsMerged OUTPUT, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	--EXEC Core.usp_AddContactMergeResult @ID, 37, @TotalRecords, @TotalRecordsMerged, @ResultCode, @ResultMessage;

	----IFSPParentGuardian 37
	--EXEC Core.usp_MergeIFSPParentGuardian @ID, @ParentID, @ChildID, @TotalRecords OUTPUT, @TotalRecordsMerged OUTPUT, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	--EXEC Core.usp_AddContactMergeResult @ID, 37, @TotalRecords, @TotalRecordsMerged, @ResultCode, @ResultMessage;

	---- Screening Contact 35
	--EXEC Core.usp_MergeScreeningContact @ID, @ParentID, @ChildID, @TotalRecords OUTPUT, @TotalRecordsMerged OUTPUT, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	--EXEC Core.usp_AddContactMergeResult @ID, 35, @TotalRecords, @TotalRecordsMerged, @ResultCode, @ResultMessage;

	
	---------------------------------------------------------------------------Consents-----------------------------------------------------------------------------		
	-- Contact Consent 73
	EXEC Core.usp_MergeContactConsent @TransactionLogID, @ContactID, @ParentID, @ChildID, @TotalRecords OUTPUT, @TotalRecordsMerged OUTPUT, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	EXEC Core.usp_AddContactMergeResult @ID, 73, @TotalRecords, @TotalRecordsMerged, @ResultCode, @ResultMessage;


	---------------------------------------------------------------------------Forms-----------------------------------------------------------------------------
	-- Contact Forms 82	
	EXEC Core.usp_MergeContactForms @TransactionLogID, @ContactID, @ParentID, @ChildID, @TotalRecords OUTPUT, @TotalRecordsMerged OUTPUT, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	EXEC Core.usp_AddContactMergeResult @ID, 82, @TotalRecords, @TotalRecordsMerged, @ResultCode, @ResultMessage;

	---------------------------------------------------------------------------Letters-----------------------------------------------------------------------------
	-- Contact Letters 81
	EXEC Core.usp_MergeContactLetters @TransactionLogID, @ContactID, @ParentID, @ChildID, @TotalRecords OUTPUT, @TotalRecordsMerged OUTPUT, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;

	EXEC Core.usp_AddContactMergeResult @ID, 81, @TotalRecords, @TotalRecordsMerged, @ResultCode, @ResultMessage;
	
	UPDATE Core.PotentialContactMatches
	SET IsActive = 0
	WHERE
		ContactID IN (@ParentID, @ChildID);

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END