-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateServiceRecording]
-- Author:		Sumana Sangapu	
-- Date:		01/28/2016
--
-- Purpose:		Update ServiceRecording
--
-- Notes:		
--
-- Depends:		
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
--	Date			Author				Notes
--	01/28/2016		Sumana Sangapu		Initial Creation
--	06/16/2016		Scott Martin		Added ParentServiceRecordingID and ServiceRecordingHeaderID
--	11/15/2016		Gurpreet Singh		Used RowVersion to control concurrency
--	12/15/2016		Scott Martin		Update auditing
--	01/11/2017		Rahul Vats			Reviewed the code to keep the following fields in Sync
--											CallCenter.CallCenterHeader.CallStartTime from Core.ServiceRecordingServiceStartDate
--											CallCenter.CallCenterHeader.CallEndTime from Core.ServiceRecordingServiceEndDate
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Core].[usp_UpdateServiceRecording]
	@ServiceRecordingID BIGINT,
	@ParentServiceRecordingID BIGINT = NULL,
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
	@SystemModifiedOn datetime,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT AS
BEGIN
	DECLARE @ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID), @AuditDetailID BIGINT, @ContactID BIGINT;
	
	SELECT @ResultCode = 0, @ResultMessage = 'executed successfully';
	
	BEGIN TRY
		SELECT @ContactID = ContactID FROM Core.vw_GetServiceRecordingDetails SRD WHERE SRD.ServiceRecordingID = @ServiceRecordingID;
		
		IF (@ServiceRecordingID IS NOT NULL)
		BEGIN
			EXECUTE [dbo].[usp_CheckRowVersion] @SystemModifiedOn , '[Core].[ServiceRecording]', '[ServiceRecordingID]', @ServiceRecordingID
		END
		
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'ServiceRecording', @ServiceRecordingID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;	

		UPDATE [Core].[ServiceRecording] SET	
			ServiceRecordingSourceID = @ServiceRecordingSourceID,
			ParentServiceRecordingID = @ParentServiceRecordingID,
			SourceHeaderID = @SourceHeaderID,
			OrganizationID = @OrganizationID,
			ServiceTypeID = @ServiceTypeID,
			ServiceItemID = @ServiceItemID,
			AttendanceStatusID = @AttendanceStatusID,
			DeliveryMethodID  = @DeliveryMethodID,
			ServiceStatusID = @ServiceStatusID,
			ServiceLocationID = @ServiceLocationID,
			ServiceStartDate = @ServiceStartDate,
			ServiceEndDate = @ServiceEndDate,
			RecipientCodeID = @RecipientCodeID,
			RecipientCode= @RecipientCode,
			NumberOfRecipients = @NumberOfRecipients,
			TrackingFieldID = @TrackingFieldID,
			SupervisorUserID = @SupervisorUserID,
			UserID = @UserID,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		WHERE ServiceRecordingID = @ServiceRecordingID;
		
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

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'ServiceRecording', @AuditDetailID, @ServiceRecordingID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(), @ResultMessage = ERROR_MESSAGE()
	END CATCH
END