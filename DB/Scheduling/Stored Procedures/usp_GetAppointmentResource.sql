-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[GetAppointmentResource]
-- Author:		John Crossen
-- Date:		10/16/2015
--
-- Purpose:		Gets the list of Appointment Resource lookup details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/16/2015	John Crossen	TFS# 2765 - Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[GetAppointmentResource]
    @AppointmentID BIGINT,
    @ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
SELECT
	@ResultCode = 0,
	@ResultMessage = 'executed successfully'

	BEGIN TRY	
	SELECT 
		AppointmentResourceID,
		AppointmentID,
		ParentID,
		ResourceID,
		ResourceTypeID,
		GroupHeaderID
	FROM
		Scheduling.AppointmentResource
	WHERE
		IsActive = 1

	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END