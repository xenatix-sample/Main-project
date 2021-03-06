
-- Procedure:	[usp_AddAppointmentReoccuring]
-- Author:		John Crossen
-- Date:		2/16/2016
--
-- Purpose:		Add Appointment Reoccuring Record
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 2/16/2016	 John Crossen    TFS#5971		Initial creation.
-- 02/17/2016	Scott Martin		Added audit logging
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [Scheduling].[usp_AddAppointmentReoccuring]
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
	@ResultMessage nvarchar(500) OUTPUT,
	@ID BIGINT OUTPUT
AS

BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

SELECT
    @ResultCode = 0,
    @ResultMessage = 'Executed Successfully';

	BEGIN TRY
	INSERT INTO [Scheduling].[AppointmentReoccuring]
	(
		[SchedulingOccurID]
		,[SchedulingFrequencyID]
		,[StartDate]
		,[EndDate]
		,[DaysOfTheWeek]
		,[IsCancelled]
		,[CancelReasonID]
		,[IsActive]
		,[ModifiedBy]
		,[ModifiedOn]
		,[CreatedBy]
		,[CreatedOn]
	) 
	VALUES
	(
		@SchedulingOccurID,
		@SchedulingFrequencyID,
		@StartDate,
		@EndDate,
		@DaysOfTheWeek,
		@IsCancelled,
		@CancelReasonID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Scheduling', 'AppointmentReoccuring', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Scheduling', 'AppointmentReoccuring', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;	

	END TRY

	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH

END