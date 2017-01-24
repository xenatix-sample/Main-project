
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetAppointmentStatus]
-- Author:		John Crossen
-- Date:		03/11/2016
--
-- Purpose:		Gets the list of Status lookup details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/11/2016	John Crossen	-   TFS# 7752   Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Reference].[usp_GetAppointmentStatus]
@ResultCode INT OUTPUT,
@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT		[AppointmentStatusID], [AppointmentStatus]
		FROM		[Reference].[AppointmentStatus] 
		WHERE		IsActive = 1
		ORDER BY	SortOrder ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END