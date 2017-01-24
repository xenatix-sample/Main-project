-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_GetServiceRecordingVoid]
-- Author:		Scott Martin
-- Date:		03/23/2016
--
-- Purpose:		Get ServiceRecording Void record
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/23/2016	Scott Martin	  - Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetServiceRecordingVoid]
	@ServiceRecordingID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
SELECT @ResultCode = 0,
		@ResultMessage = 'executed successfully'
	BEGIN TRY
	SELECT
		ServiceRecordingVoidID,
		ServiceRecordingID,
		ServiceRecordingVoidReasonID,
		(select Case when Count(*)>0 then CONVERT(BIT, 1) else CONVERT(BIT,0) end From Core.ServiceRecording Where ParentServiceRecordingID=@ServiceRecordingID) as IsCreateCopyToEdit,
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
		ModifiedBy,
		ModifiedOn

	FROM
		Core.ServiceRecordingVoid
	WHERE
		ServiceRecordingID = @ServiceRecordingID
		AND IsActive = 1;		 
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

