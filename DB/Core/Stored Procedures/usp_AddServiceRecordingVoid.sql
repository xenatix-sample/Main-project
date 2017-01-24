-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_AddServiceRecordingVoid]
-- Author:		Scott Martin
-- Date:		03/23/2016
--
-- Purpose:		Add Service Recording Void record
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/23/2016	Scott Martin		Initial creation
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_AddServiceRecordingVoid]
	@ServiceRecordingID int,
	@ServiceRecordingVoidReasonID smallint,
	@ContactID BIGINT=null,
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
		@NoteHeaderID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);
		
	SELECT @ID = SCOPE_IDENTITY();

	DECLARE @AuditDetailID BIGINT;
			
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'ServiceRecordingVoid', @ID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'ServiceRecordingVoid', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE();
	END CATCH
END