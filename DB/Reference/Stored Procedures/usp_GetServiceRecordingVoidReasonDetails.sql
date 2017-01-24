-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetServiceRecordingVoidReasonDetails]
-- Author:		Scott Martin
-- Date:		03/23/2016
--
-- Purpose:		Gets the list of ServiceRecordingVoidReason lookup details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/23/2016	Scott Martin	- Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetServiceRecordingVoidReasonDetails]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
	SELECT
		ServiceRecordingVoidReasonID,
		ServiceRecordingVoidReason  
	FROM
		[Reference].[ServiceRecordingVoidReason] 
	WHERE
		IsActive = 1
	ORDER BY
		[SortOrder]
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END