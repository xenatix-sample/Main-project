-- Procedure:	[Core].[usp_AddServiceRecordingVoidCallCenter]
-- Author:		Deepak Kumar
-- Date:		06/22/2016
--
-- Purpose:		Add Service Recording Void for call center
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/22/2016	Deepak Kumar		Initial creation
-- 08/19/2016   Gaurav Gupta	    Update void status in call center 
-- 11/08/2016   Gaurav Gupta	    check response id in assessment response details table 
-- 12/05/2016   Gaurav Gupta		Dynamic assement ids based on crisis line and law liaison.
-- 12/05/2016   Gaurav Gupta        Passing null parent header id, should only pass for follow up record.
-- 12/15/2016	Scott Martin		Updated auditing
-- 12/22/2016   Vishal Yadav        Save Contact TypeID for draft copy.
-- 12/15/2016	Scott Martin	    Updated auditing
-- 12/23/2016   Arun Choudhary      Removed Digital Signature Input Type ID from AssessmentResponseDetails
-- 01/04/2017   Gaurav Gupta        Copy Prgoress Note Data 
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_AddServiceRecordingVoidCallCenter]
	@IsCreateCopyToEdit bit,
	@ServiceRecordingID int,
	@ServiceRecordingVoidReasonID smallint,
	@ContactID bigint,
	@IncorrectOrganization bit,
	@IncorrectServiceType bit,
	@IncorrectServiceItem bit,
	@IncorrectServiceStatus bit,
	@IncorrectSupervisor bit,
	@IncorrectAdditionalUser bit,
	@IncorrectAttendanceStatus bit,
	@IncorrectStartDate bit,
	@IncorrectStartTime bit,
	@IncorrectEndDate bit,
	@IncorrectEndTime bit,
	@IncorrectDeliveryMethod bit,
	@IncorrectServiceLocation bit,
	@IncorrectRecipientCode bit,
	@IncorrectTrackingField bit,
	@Comments nvarchar(1000),
	@NoteHeaderID BIGINT,
	@ModifiedOn datetime,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID BIGINT OUTPUT

AS
BEGIN
	DECLARE @ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);
	DECLARE @CallCenterHeaderID BIGINT
	DECLARE @AuditDetailID BIGINT;
	DECLARE @ResponseID BIGINT
	DECLARE @SECTIONID BIGINT;
	Declare @ServiceRecordingVoidID bigint;
    DECLARE @ASSESSMENT_IDS VARCHAR(30);
	DECLARE @NoteContactID BIGINT;

    SELECT @ASSESSMENT_IDS= CASE WHEN CH.CallCenterTypeID = 1 THEN '6,7,8,9,11'  ELSE '10,12' END FROM CallCenter.CallCenterHeader CH where CallCenterHeaderID=@NoteHeaderID and IsActive=1

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	INSERT INTO  Core.ServiceRecordingVoid
	(
		ServiceRecordingID,
		ServiceRecordingVoidReasonID,
		IncorrectOrganization,
		IncorrectServiceType,
		IncorrectServiceItem,
		IncorrectServiceStatus,
		IncorrectSupervisor,
		IncorrectAdditionalUser,
		IncorrectAttendanceStatus,
		IncorrectStartDate,
		IncorrectStartTime,
		IncorrectEndDate,
		IncorrectEndTime,
		IncorrectDeliveryMethod,
		IncorrectServiceLocation,
		IncorrectRecipientCode,
		IncorrectTrackingField,
		Comments,
		NoteHeaderID,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	VALUES
	(
		@ServiceRecordingID,
		@ServiceRecordingVoidReasonID,
		@IncorrectOrganization,
		@IncorrectServiceType,
		@IncorrectServiceItem,
		@IncorrectServiceStatus,
		@IncorrectSupervisor,
		@IncorrectAdditionalUser,
		@IncorrectAttendanceStatus,
		@IncorrectStartDate,
		@IncorrectStartTime,
		@IncorrectEndDate,
		@IncorrectEndTime,
		@IncorrectDeliveryMethod,
		@IncorrectServiceLocation,
		@IncorrectRecipientCode,
		@IncorrectTrackingField,
		@Comments,
		null,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);
		
	SELECT @ServiceRecordingVoidID = SCOPE_IDENTITY();

	
	
	If(@IsCreateCopyToEdit=0)
	
	Begin
	UPDATE CallCenter.CallCenterHeader set CallStatusID=6
	WHERE CallCenterHeaderID=@NoteHeaderID and IsActive=1
	SELECT @ID = SCOPE_IDENTITY();
	End

	Else
	Begin
	IF Exists(select 1 from CallCenter.CallCenterHeader where CallCenterHeaderID=@NoteHeaderID and IsActive=1)
	Begin
	INSERT INTO CallCenter.CallCenterHeader
	(		ParentCallCenterHeaderID
           ,CallCenterTypeID
		   ,ContactTypeID
           ,CallerID
           ,ContactID
           ,ProviderID
           ,CallStartTime
           ,CallEndTime
           ,CallStatusID
           ,ProgramUnitID
           ,CountyID
           ,IsLinkedToContact
           ,IsActive
           ,ModifiedBy
           ,ModifiedOn
           ,CreatedBy
           ,CreatedOn
           ,SystemCreatedOn
           ,SystemModifiedOn
	)
     SELECT
			null AS ParentCallCenterHeaderID
           ,CallCenterTypeID
		   ,ContactTypeID
           ,CallerID
           ,ContactID
           ,ProviderID
           ,CallStartTime
           ,CallEndTime
           ,5
           ,ProgramUnitID
           ,CountyID
           ,IsLinkedToContact
           ,1
           ,@ModifiedBy
           ,@ModifiedOn
           ,CreatedBy
           ,@ModifiedOn
           ,SystemCreatedOn
           ,SystemModifiedOn
	FROM
			CallCenter.CallCenterHeader
	WHERE	CallCenterHeaderID = @NoteHeaderID
			AND IsActive=1
	
	SELECT @ID = SCOPE_IDENTITY();
	SET @CallCenterHeaderID=@ID
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'CallCenter', 'CallCenterHeader', @ID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'CallCenter', 'CallCenterHeader', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	END

	UPDATE CallCenter.CallCenterHeader set CallStatusID=6
	WHERE CallCenterHeaderID=@NoteHeaderID and IsActive=1
	


	INSERT INTO CallCenter.CrisisCall
           (
		   CallCenterHeaderID
           ,CallCenterPriorityID
           ,SuicideHomicideID
           ,DateOfIncident
           ,ReasonCalled
           ,Disposition
           ,OtherInformation
           ,Comments
           ,FollowUpRequired
           ,CallTypeID
           ,CallTypeOther
           ,FollowupPlan
           ,NatureofCall
           ,ClientStatusID
           ,ClientProviderID
           ,BehavioralCategoryID
           ,NoteHeaderID
           ,ReferralAgencyID
           ,OtherReferralAgency
           ,IsActive
           ,ModifiedBy
           ,ModifiedOn
           ,CreatedBy
           ,CreatedOn
           ,SystemCreatedOn
           ,SystemModifiedOn
		   )
	SELECT
		    @CallCenterHeaderID
           ,CallCenterPriorityID
           ,SuicideHomicideID
           ,DateOfIncident
           ,ReasonCalled
           ,Disposition
           ,OtherInformation
           ,Comments
           ,FollowUpRequired
           ,CallTypeID
           ,CallTypeOther
           ,FollowupPlan
           ,NatureofCall
           ,ClientStatusID
           ,ClientProviderID
           ,BehavioralCategoryID
           ,NoteHeaderID
           ,ReferralAgencyID
           ,OtherReferralAgency
           ,1
           ,@ModifiedBy
           ,@ModifiedOn
           ,CreatedBy
           ,@ModifiedOn
           ,SystemCreatedOn
           ,SystemModifiedOn
	FROM
			CallCenter.CrisisCall
	Where
			CallCenterHeaderID=@NoteHeaderID
			AND IsActive=1

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'CallCenter', 'CrisisCall', @ID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'CallCenter', 'CrisisCall', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	IF Exists(SELECT 1 FROM CallCenter.ProgressNote WHERE CallCenterHeaderID=@NoteHeaderID AND IsActive=1)
	BEGIN

	INSERT INTO Registration.NoteHeader
						( ContactID ,
						  NoteTypeID ,
						  TakenBy ,
						  TakenTime ,
						  IsActive ,
						  ModifiedBy ,
						  ModifiedOn ,
						  CreatedBy ,
						  CreatedOn
						)
				SELECT 
				          ContactID ,
						  NoteTypeID ,
						  TakenBy ,
						  TakenTime ,
								1,
						  @ModifiedBy,
						  @ModifiedOn,
						  @ModifiedBy,
						  @ModifiedOn
			FROM Registration.NoteHeader 
		    WHERE ContactID=@ContactID
			      AND IsActive=1;
	  
	DECLARE @ProgressNoteHeaderID BIGINT = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'NoteHeader', @ProgressNoteHeaderID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'NoteHeader', @AuditDetailID, @ProgressNoteHeaderID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	IF Exists(SELECT 1 FROM Registration.Contact WHERE ContactID=(SELECT NewCallerID FROM CallCenter.ProgressNote WHERE CallCenterHeaderID=@NoteHeaderID AND IsActive=1) AND IsActive=1)
	BEGIN
		INSERT INTO Registration.Contact(
					ContactTypeID, 
					ClientTypeID, 
					FirstName, 
					Middle, 
					LastName, 
					SuffixID, 
					GenderID, 
					PreferredGenderID,
					TitleID, 
					SequesteredByID, 
					DOB, 
					DOBStatusID,
					SSN,
					SSNStatusID,
					DriverLicense, 
					DriverLicenseStateID,
					PreferredName,
					DeceasedDate, 
					PreferredContactMethodID, 
					ReferralSourceID, 
					IsActive, 
					ModifiedBy, 
					ModifiedOn,
					IsPregnant,
					GestationalAge,
					CreatedBy,
					CreatedOn,
					SSNEncrypted,
					IsDeceased,
					CauseOfDeath
				)
				SELECT
					ContactTypeID, 
					ClientTypeID, 
					FirstName, 
					Middle, 
					LastName, 
					SuffixID, 
					GenderID, 
					PreferredGenderID,
					TitleID, 
					SequesteredByID, 
					DOB, 
					DOBStatusID,
					SSN,
					SSNStatusID,
					DriverLicense, 
					DriverLicenseStateID,
					PreferredName,
					DeceasedDate, 
					PreferredContactMethodID, 
					ReferralSourceID, 
					IsActive, 
					ModifiedBy, 
					ModifiedOn,
					IsPregnant,
					GestationalAge,
					CreatedBy,
					CreatedOn,
					SSNEncrypted,
					IsDeceased,
					CauseOfDeath

				  FROM Registration.Contact
				  WHERE ContactID=(SELECT NewCallerID FROM CallCenter.ProgressNote WHERE CallCenterHeaderID=@NoteHeaderID AND IsActive=1)
				  AND IsActive=1
					SET @NoteContactID = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'Contact', @NoteContactID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'Contact', @AuditDetailID, @NoteContactID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END
	ELSE
		SET @NoteContactID = @ContactID;
    
	
    INSERT INTO CallCenter.ProgressNote
				(
					[CallCenterHeaderID],
					[NoteHeaderID],
					[Disposition],
					[Comments],
					[CallTypeID],
					[CallTypeOther],
					[FollowupPlan],
					[NatureofCall],
					[IsCallerSame],
					[NewCallerID],
					[IsActive],
					[ModifiedBy],
					[ModifiedOn],
					[CreatedBy],
					[CreatedOn]
				)
				SELECT
					@CallCenterHeaderID,
					@ProgressNoteHeaderID,
					[Disposition],
					[Comments],
					[CallTypeID],
					[CallTypeOther],
					[FollowupPlan],
					[NatureofCall],
					[IsCallerSame],
					@NoteContactID,
					[IsActive],
					[ModifiedBy],
					[ModifiedOn],
					[CreatedBy],
					[CreatedOn]

				FROM CallCenter.ProgressNote
				WHERE CallCenterHeaderID=@NoteHeaderID
				AND IsActive=1
	
	DECLARE @ProgressNoteID BIGINT = SCOPE_IDENTITY();
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'CallCenter', 'ProgressNote', @ProgressNoteID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'CallCenter', 'ProgressNote', @AuditDetailID, @ProgressNoteID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	IF Exists(SELECT 1 FROM Registration.ContactPhone WHERE ContactID=(SELECT NewCallerID FROM CallCenter.ProgressNote WHERE CallCenterHeaderID=@NoteHeaderID AND IsActive=1) AND IsActive=1)
	BEGIN
		DECLARE @PhoneID BIGINT =(select Top 1 PhoneID from Registration.ContactPhone WHERE ContactID=(SELECT NewCallerID FROM CallCenter.ProgressNote WHERE CallCenterHeaderID=@NoteHeaderID AND IsActive=1));

		INSERT INTO Registration.ContactPhone
		(
			ContactID,
			PhoneID,
			PhonePermissionID,
			IsPrimary,
			EffectiveDate,
			ExpirationDate,
			IsActive,
			ModifiedBy,
			ModifiedOn,
			CreatedBy,
			CreatedOn
		)
		SELECT @NoteContactID,
			@PhoneID,
			PhonePermissionID,
			IsPrimary,
			EffectiveDate,
			ExpirationDate,
			IsActive,
			ModifiedBy,
			ModifiedOn,
			CreatedBy,
			CreatedOn
			
			FROM Registration.ContactPhone
			WHERE ContactID=(SELECT NewCallerID FROM CallCenter.ProgressNote WHERE CallCenterHeaderID=@NoteHeaderID AND IsActive=1)
			      AND IsActive=1;
	DECLARE @ContactPhoneID BIGINT = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'ContactPhone', @ContactPhoneID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'ContactPhone', @AuditDetailID, @ContactPhoneID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

    END


	END

	IF Exists(select 1 from Core.ServiceRecording where ServiceRecordingID = @ServiceRecordingID and IsActive=1)
	Begin
	INSERT INTO Core.ServiceRecordingHeader
    (
           IsActive
           ,ModifiedBy
           ,ModifiedOn
           ,CreatedBy
           ,CreatedOn
           ,SystemCreatedOn
           ,SystemModifiedOn
	)
	VALUES
		( 1
           ,@ModifiedBy
           ,GETUTCDATE()
           ,@ModifiedBy
           ,GETUTCDATE()
           ,GETUTCDATE()
           ,GETUTCDATE())

	SELECT @ID = SCOPE_IDENTITY();
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'ServiceRecordingHeader', @ID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'ServiceRecordingHeader', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;



	INSERT INTO Core.ServiceRecording
    (
		   ServiceRecordingHeaderID
           ,ParentServiceRecordingID
           ,ServiceRecordingSourceID
           ,SourceHeaderID
           ,OrganizationID
           ,ServiceTypeID
           ,ServiceItemID
           ,AttendanceStatusID
           ,ServiceStatusID
           ,RecipientCodeID
           ,RecipientCode
           ,DeliveryMethodID
           ,ServiceLocationID
           ,ServiceStartDate
           ,ServiceEndDate
           ,TransmittalStatus
           ,NumberOfRecipients
           ,TrackingFieldID
           ,SupervisorUserID
           ,UserID
           ,IsActive
           ,ModifiedBy
           ,ModifiedOn
           ,CreatedBy
           ,CreatedOn
           ,SystemCreatedOn
           ,SystemModifiedOn
	)
	SELECT
			@ID
           ,ServiceRecordingID AS ParentServiceRecordingID
           ,ServiceRecordingSourceID
           ,@CallCenterHeaderID
           ,OrganizationID
           ,ServiceTypeID
           ,ServiceItemID
           ,AttendanceStatusID
           ,ServiceStatusID
           ,RecipientCodeID
           ,RecipientCode
           ,DeliveryMethodID
           ,ServiceLocationID
           ,ServiceStartDate
           ,ServiceEndDate
           ,TransmittalStatus
           ,NumberOfRecipients
           ,TrackingFieldID
           ,SupervisorUserID
           ,UserID
           ,1
           ,@ModifiedBy
           ,@ModifiedOn
           ,CreatedBy
           ,@ModifiedOn
           ,SystemCreatedOn
           ,SystemModifiedOn
	FROM
			Core.ServiceRecording
	WHERE
			ServiceRecordingID = @ServiceRecordingID
			AND IsActive=1
	
	SELECT @ID = SCOPE_IDENTITY();

	/********* Service Recording additional user********/
	IF Exists(select 1 from Core.ServiceRecordingAdditionalUser where ServiceRecordingID = @ServiceRecordingID and IsActive=1)
	BEGIN
	INSERT INTO Core.ServiceRecordingAdditionalUser
	(
		ServiceRecordingID,
		UserID,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	select 
		@ID,
		UserID,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	FROM
			Core.ServiceRecordingAdditionalUser
	WHERE
			ServiceRecordingID = @ServiceRecordingID
			AND IsActive=1
	
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'ServiceRecordingAdditionalUser', @ID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'ServiceRecordingAdditionalUser', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
    END


	/********* Service Recording Attendent********/
	IF Exists(select 1 from Core.ServiceRecordingAttendee where ServiceRecordingID = @ServiceRecordingID and IsActive=1)
	BEGIN
	INSERT INTO  Core.ServiceRecordingAttendee
	(
		ServiceRecordingID,
		Name,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	select 
		@ID,
		Name,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	FROM
			Core.ServiceRecordingAttendee
	WHERE
			ServiceRecordingID = @ServiceRecordingID
			AND IsActive=1
	
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'ServiceRecordingAttendee', @ID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'ServiceRecordingAttendee', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	END
	
	END
	

	DECLARE @AssessmentTemp TABLE(ID INT IDENTITY,AssessmentID INT,Name NVARCHAR(50))
	DECLARE @Maxid int
	DECLARE @AssessmentID Bigint
	INSERT INTO @AssessmentTemp(AssessmentID,Name)
	SELECT AssessmentID,Name FROM Core.Assessments where AssessmentID IN (select * from Core.fn_Split (@ASSESSMENT_IDS, ','))
	Select @Maxid=MAX(ID) FROM @AssessmentTemp

	WHILE (@Maxid>0)
		Begin
			Select @AssessmentID=AssessmentID From @AssessmentTemp WHERE ID=@Maxid
			IF Exists(select 1 from Core.AssessmentResponses where AssessmentID=@AssessmentID AND ContactID=@ContactID AND IsActive=1)
			Begin

		 DECLARE @PreviousResponseID BIGINT
			select @PreviousResponseID=ResponseID from Core.AssessmentResponses
				WHERE AssessmentID=@AssessmentID AND ContactID=@ContactID AND IsActive=1


			INSERT INTO Core.AssessmentResponses
			(
					ContactID
					,AssessmentID
					,EnterDate
					,EnterDateINT
					,IsActive
					,ModifiedBy
					,ModifiedOn
					,CreatedBy
					,CreatedOn
					,SystemCreatedOn
					,SystemModifiedOn
			)
			SELECT  top 1
					ContactID
					,AssessmentID
					,EnterDate
					,EnterDateINT
					,IsActive
					,@ModifiedBy
					,@ModifiedOn
					,CreatedBy
					,@ModifiedOn
					,SystemCreatedOn
					,SystemModifiedOn
			FROM	core.AssessmentResponses 
			WHERE	
					AssessmentID=@AssessmentID 
					AND ContactID=@ContactID
					AND IsActive=1

			SELECT @ID = SCOPE_IDENTITY();	
			SET @ResponseID=@ID --ResponseID NEW

			EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'core', 'AssessmentResponses', @ID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			EXEC Auditing.usp_AddPostAuditLog 'Insert', 'core', 'AssessmentResponses', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			

			INSERT INTO CallCenter.CallCenterAssessmentResponse
			(
				CallCenterHeaderID
				,AssessmentID
				,ResponseID
				,IsActive
				,ModifiedBy
				,ModifiedOn
				,CreatedBy
				,CreatedOn
				,SystemCreatedOn
				,SystemModifiedOn	
			)
			SELECT
				@CallCenterHeaderID
				,AssessmentID 
				,@ResponseID
				,IsActive
				,@ModifiedBy
				,@ModifiedOn
				,CreatedBy
				,@ModifiedOn
				,SystemCreatedOn
				,SystemModifiedOn	
			FROM
				CallCenter.CallCenterAssessmentResponse
			WHERE
				AssessmentID=@AssessmentID
				AND CallCenterHeaderID= @NoteHeaderID
				AND IsActive=1
				AND ResponseID=@PreviousResponseID
	
			SELECT @ID = SCOPE_IDENTITY();	
			
			EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'CallCenter', 'CallCenterAssessmentResponse', @ID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			EXEC Auditing.usp_AddPostAuditLog 'Insert', 'CallCenter', 'CallCenterAssessmentResponse', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			
			SELECT @SECTIONID=AssessmentSectionID
			FROM	Core.AssessmentSections
			WHERE	AssessmentID=@AssessmentID
					AND IsActive=1
			IF(@SECTIONID IS NOT NULL)
			Begin
			INSERT INTO core.AssessmentResponseDetails
			(
				ResponseID
				,QuestionID
				,AssessmentSectionID
				,OptionsID
				,ResponseText
				,Rating
				,IsActive
				,ModifiedBy
				,ModifiedOn
				,CreatedBy
				,CreatedOn
				,SystemCreatedOn
				,SystemModifiedOn
			)
			SELECT
				@ResponseID
				,ARD.QuestionID
				,ARD.AssessmentSectionID
				,OptionsID
				,ResponseText
				,Rating
				,ARD.IsActive
				,@ModifiedBy
				,@ModifiedOn
				,ARD.CreatedBy
				,@ModifiedOn
				,ARD.SystemCreatedOn
				,ARD.SystemModifiedOn
			FROM
				core.AssessmentResponseDetails ARD
				INNER JOIN Core.AssessmentQuestions AQ
			ON ARD.QuestionID = AQ.QuestionID
			WHERE
				ARD.AssessmentSectionID=@SECTIONID
				AND ARD.IsActive=1
				AND ARD.ResponseID=@PreviousResponseID
				AND AQ.InputTypeID <> 12
			
			SELECT @ID = SCOPE_IDENTITY();	
			
			EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'core', 'AssessmentResponseDetails', @ID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			EXEC Auditing.usp_AddPostAuditLog 'Insert', 'core', 'AssessmentResponseDetails', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			END
			END
			SET @Maxid=@Maxid-1
		END

End
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE();
	END CATCH
END


