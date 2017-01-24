-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_AddServiceRecording]
-- Author:		Sumana Sangapu
-- Date:		01/28/2016
--
-- Purpose:		Add Service Recording
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
--		Date				Author									Notes
--		01/28/2016			Sumana Sangapu							Initial creation.
--		02/17/2016			Scott Martin							Refactored for audit logging
--		03/23/2016			Scott Martin							Added new fields and parameters
--		06/17/2016			Scott Martin							Added code for creating a Service Header record and added TrackingFieldID
--		11/22/2016			Atul Chauhan							Added code for handling concurrency 
--		12/15/2016			Scott Martin							Updated auditing
--		01/11/2017			Rahul Vats								Reviewed the code to keep the following fields in Sync
--																		CallCenter.CallCenterHeader.CallStartTime from Core.ServiceRecordingServiceStartDate
--																		CallCenter.CallCenterHeader.CallEndTime from Core.ServiceRecordingServiceEndDate
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Core].[usp_AddServiceRecording]
	@ServiceRecordingHeaderID bigint = NULL,
	@ParentServiceRecordingID bigint = NULL,
	@ServiceRecordingSourceID int,
 @SourceHeaderID bigint,
	@OrganizationID bigint,
	@ServiceTypeID smallint,
	@ServiceItemID smallint,
	@AttendanceStatusID smallint,
	@DeliveryMethodID  smallint,
	@ServiceStatusID smallint,
	@ServiceLocationID smallint,
	@ServiceStartDate datetime,
	@ServiceEndDate datetime,
	@RecipientCodeID smallint ,
 @RecipientCode smallint,
	@NumberOfRecipients smallint,
	@TrackingFieldID int,
	@SupervisorUserID int,
	@UserID int,
	@ModifiedOn datetime,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID BIGINT OUTPUT AS
BEGIN
	DECLARE @ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID), @AuditDetailID BIGINT, @ContactID BIGINT;
	BEGIN TRY
		SELECT @ResultCode = 0, @ResultMessage = 'executed successfully';
		IF EXISTS(SELECT * FROM Core.ServiceRecording WHERE SourceHeaderID = @SourceHeaderID AND ServiceRecordingSourceID = @ServiceRecordingSourceID)
		BEGIN
			RAISERROR('Data has been changed. Please reload the page.', 15, 1);
		END
		IF ISNULL(@ServiceRecordingHeaderID, 0) = 0
		BEGIN
			INSERT INTO Core.ServiceRecordingHeader ( IsActive, ModifiedBy,	ModifiedOn,	CreatedBy,	CreatedOn )
			VALUES ( 1, @ModifiedBy, @ModifiedOn, @ModifiedBy,@ModifiedOn )

			SELECT @ServiceRecordingHeaderID = SCOPE_IDENTITY();

			EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'ServiceRecordingHeader', @ServiceRecordingHeaderID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; --This needs to be reviewed

			EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'ServiceRecordingHeader', @AuditDetailID, @ServiceRecordingHeaderID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		END

		INSERT INTO  Core.ServiceRecording (
			ServiceRecordingHeaderID, ParentServiceRecordingID, ServiceRecordingSourceID, SourceHeaderID,
			OrganizationID, ServiceTypeID, ServiceItemID, AttendanceStatusID,
			ServiceStatusID, RecipientCodeID, RecipientCode, DeliveryMethodID,
			ServiceLocationID, ServiceStartDate, ServiceEndDate, TransmittalStatus,
			NumberOfRecipients, TrackingFieldID, SupervisorUserID, UserID,
			IsActive, ModifiedBy, ModifiedOn, CreatedBy,
		CreatedOn )
		VALUES (
			@ServiceRecordingHeaderID, @ParentServiceRecordingID, @ServiceRecordingSourceID, @SourceHeaderID,
			@OrganizationID, @ServiceTypeID, @ServiceItemID, @AttendanceStatusID,
			@ServiceStatusID, @RecipientCodeID, @RecipientCode, @DeliveryMethodID,
			@ServiceLocationID, @ServiceStartDate, @ServiceEndDate,	NULL,
			@NumberOfRecipients, @TrackingFieldID, @SupervisorUserID, @UserID,
			1, @ModifiedBy, @ModifiedOn, @ModifiedBy,
		@ModifiedOn );
		
		SELECT @ID = SCOPE_IDENTITY();
        SELECT @ContactID = ContactID FROM Core.vw_GetServiceRecordingDetails SRD WHERE SRD.ServiceRecordingID = @ID;

		Declare @CrisisServiceRecordSourceID int, @CallCenterTypeID int;
		Select @CrisisServiceRecordSourceID = ServiceRecordingSourceID from Reference.ServiceRecordingSource where ServiceRecordingSource = 'CallCenter';
  Select @CallCenterTypeID = CallCenterTypeID from CallCenter.CallCenterType where CallCenterType = 'Crisis Line';
		If (@ServiceRecordingSourceID = @CrisisServiceRecordSourceID)
		Begin
    -- Add Pre Audit Log For Call Center Update
    EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'CallCenter', 'CallCenterHeader', @SourceHeaderID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			
   --Select 'Crisis Line Selected'
   UPDATE CallCenter.CallCenterHeader SET CallStartTime = @ServiceStartDate, CallEndTime = @ServiceEndDate,
    ModifiedBy=@ModifiedBy,	ModifiedOn=@ModifiedOn, SystemModifiedOn=GETUTCDATE()
			WHERE CallCenterHeaderID = @SourceHeaderID and CallCenterTypeID = @CallCenterTypeID;
   
   -- Add Post Audit Log For Call Center Update
   EXEC Auditing.usp_AddPostAuditLog 'Update', 'CallCenter', 'CallCenterHeader', @AuditDetailID, @SourceHeaderID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
  End
		
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'ServiceRecording', @ID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'ServiceRecording', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(), @ResultMessage = ERROR_MESSAGE(), @ID=0;
	END CATCH
END