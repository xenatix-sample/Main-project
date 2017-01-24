
-- Procedure:	[usp_UpdateAppointmentReoccuring]
-- Author:		John Crossen
-- Date:		2/16/2016
--
-- Purpose:		Update Appointment Reoccuring Record
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 2/16/2016	 John Crossen    TFS#5971		Initial creation.
-- 02/17/2016	Scott Martin		Added audit logging
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_UpdateAppointmentReoccuring]
	@AppointmentReoccuringID BIGINT,
	@SchedulingOccurID INT,
	@SchedulingFrequencyID INT,
	@StartDate DATE,
	@EndDate DATE,
	@DaysOfTheWeek NVARCHAR(20),
	@IsCancelled BIT,
	@CancelReasonID INT,
	@ModifiedBy INT,
	@ModifiedOn DATETIME,
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT
AS

BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

SELECT
    @ResultCode = 0,
    @ResultMessage = 'Executed Successfully'

	BEGIN TRY
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Scheduling', 'AppointmentReoccuring', @AppointmentReoccuringID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE [Scheduling].[AppointmentReoccuring]    
	SET [SchedulingOccurID] = @SchedulingOccurID,
		[SchedulingFrequencyID] = @SchedulingFrequencyID,
		[StartDate] = @StartDate,
		[EndDate] = @EndDate, 
		[DaysOfTheWeek] = @DaysOfTheWeek,
		[IsCancelled] = @IsCancelled,
		[CancelReasonID] = @CancelReasonID,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		[AppointmentReoccuringID] = @AppointmentReoccuringID

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Scheduling', 'AppointmentReoccuring', @AuditDetailID, @AppointmentReoccuringID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH

END