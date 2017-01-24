-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_UpdateServiceRecordingVoid]
-- Author:		Scott Martin
-- Date:		03/23/2016
--
-- Purpose:		Update Service Recording Void record
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/23/2016	Scott Martin		Initial creation
-- 12/15/2016	Scott Martin		Update auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_UpdateServiceRecordingVoid]
	@ServiceRecordingVoidID bigint,
	@ServiceRecordingID int,
	@ServiceRecordingVoidReasonID smallint,
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
	@NoteHeaderID bigint,
	@ModifiedOn datetime,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@AuditDetailID BIGINT,
		@ContactID BIGINT;

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	BEGIN TRY
	SELECT @ContactID = ContactID FROM Core.vw_GetServiceRecordingDetails SRD WHERE SRD.ServiceRecordingID = @ServiceRecordingID;

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'ServiceRecordingVoid', @ServiceRecordingVoidID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT

	UPDATE  Core.ServiceRecordingVoid
	SET ServiceRecordingID = @ServiceRecordingID,
		ServiceRecordingVoidReasonID = @ServiceRecordingVoidReasonID,
		IncorrectOrganization = @IncorrectOrganization,
		IncorrectServiceType = @IncorrectServiceType,
		IncorrectServiceItem = @IncorrectServiceItem,
		IncorrectServiceStatus = @IncorrectServiceStatus,
		IncorrectSupervisor = @IncorrectSupervisor,
		IncorrectAdditionalUser = @IncorrectAdditionalUser,
		IncorrectAttendanceStatus = @IncorrectAttendanceStatus,
		IncorrectStartDate = @IncorrectStartDate,
		IncorrectStartTime = @IncorrectStartTime,
		IncorrectEndDate = @IncorrectEndDate,
		IncorrectEndTime = @IncorrectEndTime,
		IncorrectDeliveryMethod = @IncorrectDeliveryMethod,
		IncorrectServiceLocation = @IncorrectServiceLocation,
		IncorrectRecipientCode = @IncorrectRecipientCode,
		IncorrectTrackingField = @IncorrectTrackingField,
		Comments = @Comments,
		NoteHeaderID = @NoteHeaderID,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ServiceRecordingVoidID = @ServiceRecordingVoidID;
		

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'ServiceRecordingVoid', @AuditDetailID, @ServiceRecordingVoidID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE();
	END CATCH
END