-- Procedure:	[usp_DeleteAppointmentsByRecurrenceID]
-- Author:		Chad Roberts
-- Date:		04/01/2016
--
-- Purpose:		Used for cleaning up appointments when a recurrence has changed
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_DeleteAppointmentsByRecurrenceID]
	@RecurrenceID BIGINT,
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT
AS

BEGIN
SELECT
    @ResultCode = 0,
    @ResultMessage = 'Executed Successfully'

	BEGIN TRY
		BEGIN
			update Scheduling.Appointment
			set IsActive = 0
			where RecurrenceID = @RecurrenceID and AppointmentDate > current_timestamp

			update ac
			set ac.IsActive = 0
			from Scheduling.Appointment a
				inner join Scheduling.AppointmentContact ac on a.AppointmentID = ac.AppointmentID
			where a.RecurrenceID = @RecurrenceID and a.AppointmentDate > current_timestamp

			update ar
			set ar.IsActive = 0
			from Scheduling.Appointment a
				inner join Scheduling.AppointmentResource ar on a.AppointmentID = ar.AppointmentID
			where a.RecurrenceID = @RecurrenceID and a.AppointmentDate > current_timestamp
		END

	END TRY

	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH

END