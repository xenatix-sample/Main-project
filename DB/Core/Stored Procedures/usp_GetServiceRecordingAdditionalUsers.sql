-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_GetServiceRecordingAdditionalUsers]
-- Author:		Scott Martin
-- Date:		03/23/2016
--
-- Purpose:		Get ServiceRecording Additional Users
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/23/2016	Scott Martin	  - Initial creation.
-- 08/16/2016	Vishal Yadav Removed IsActive check.
-- 08/22/2016	Vishal Yadav Added back IsActive check.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetServiceRecordingAdditionalUsers]
	@ServiceRecordingID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
SELECT @ResultCode = 0,
		@ResultMessage = 'executed successfully'
	BEGIN TRY
	SELECT
		ServiceRecordingAdditionalUserID,
		ServiceRecordingID,
		UserID,
		IsActive
	FROM
		Core.ServiceRecordingAdditionalUser
	WHERE
		ServiceRecordingID = @ServiceRecordingID AND
		IsActive = 1
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

