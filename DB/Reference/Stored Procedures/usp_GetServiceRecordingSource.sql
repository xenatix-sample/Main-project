
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetServiceRecordingSource]
-- Author:		Sumana Sangapu
-- Date:		01/28/2016
--
-- Purpose:		Gets the list of Service Recording Source lookup details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/28/2016	Sumana Sangapu	- Initial creation.
-- 06/29/2016	Gurpreet Singh	Added DisplayText
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [Reference].[usp_GetServiceRecordingSource]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		ServiceRecordingSourceID, ServiceRecordingSource, DisplayText
		FROM		[Reference].[ServiceRecordingSource] 
		WHERE		IsActive = 1
		ORDER BY	ServiceRecordingSource ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END