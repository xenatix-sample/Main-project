-- Procedure:	[usp_GetAppointmentReoccuring]
-- Author:		John Crossen
-- Date:		2/16/2016
--
-- Purpose:		Get Appointment Reoccuring Record
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 2/16/2016	 John Crossen    TFS#5971		Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_GetAppointmentReoccuring]
	@AppointmentReoccuringID BIGINT,
	@ModifiedBy INT,
	@ModifiedOn DATETIME,
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT
AS

BEGIN
SELECT
	@ResultCode = 0,
	@ResultMessage = 'Executed Successfully'

	BEGIN TRY
	SELECT
		[AppointmentReoccuringID],
		[SchedulingOccurID],
		[SchedulingFrequencyID],
		[StartDate],
		[EndDate],
		[DaysOfTheWeek],
		[IsCancelled],
		[CancelReasonID],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn],
		[CreatedBy],
		[CreatedOn],
		[SystemCreatedOn],
		[SystemModifiedOn]
	FROM
		[Scheduling].[AppointmentReoccuring]
	WHERE
		[AppointmentReoccuringID] = @AppointmentReoccuringID

	END TRY

	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH

END