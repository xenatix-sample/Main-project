-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetAppointmentLengths]
-- Author:		John Crossen
-- Date:		10/02/2015
--
-- Purpose:		Gets the list of Appointment Length lookup details based on Program and Appointment Type
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/11/2015	John Crossen	TFS# 2583 - Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_GetAppointmentLengths]
	@AppointmentTypeID INT,
    @ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
			@ResultMessage = 'executed successfully'

	BEGIN TRY	
		SELECT
			AL.AppointmentLengthID,
			AT.AppointmentTypeID,
			AL.AppointmentLength
		FROM
			[Scheduling].[AppointmentLength] AL 
			JOIN [Scheduling].[AppointmentType] AT
				ON AL.AppointmentTypeID = AT.AppointmentTypeID
		WHERE
			AL.IsActive = 1
			AND AT.AppointmentTypeID = @AppointmentTypeID
		ORDER BY
			AppointmentLengthID ASC
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


