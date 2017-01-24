-- Procedure:	[usp_GetAppointmentsByDate]
-- Author:		Scott Martin
-- Date:		11/25/2015
--
-- Purpose:		Get all appointments for a date range
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/25/2015	Scot Martin    TFS#3411		Initial creation.
-- 02/17/2016	Scott Martin	Added IsCancelled parameter to select statement
-- 03/02/2016	Scott Martin	Added CancelReasonID, CancelComment, and Comments
-- 04/11/2016	Sumana Sangapu	Modified the proc to use View - vw_GetAppointmentDetails
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE Scheduling.usp_GetAppointmentsByDate
	@StartDate DATE,
	@EndDate DATE = NULL,
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT
AS

BEGIN
  SELECT
    @ResultCode = 0,
    @ResultMessage = 'Executed Successfully'

  BEGIN TRY

	IF @EndDate IS NULL
		BEGIN
		SET @EndDate = @StartDate;
		END
 
	SELECT 
		[AppointmentID],
		[ProgramID],
		[AppointmentTypeID],
		[FacilityID],
		[ServicesID],
		[AppointmentDate],
		[AppointmentStartTime],
		[AppointmentLength],
		[SupervisionVisit],
		[ReferredBy],
		[ReasonForVisit],
		[IsCancelled],
		[CancelReasonID],
		[CancelComment],
		[Comments],
		[ModifiedBy],
		[ModifiedOn]
	FROM
		Scheduling.vw_GetAppointmentDetails 
	WHERE
		AppointmentDate BETWEEN @StartDate AND @EndDate
		AND IsActive = 1
		AND IsCancelled = 0;

   END TRY
  BEGIN CATCH
    SELECT
      @ResultCode = ERROR_SEVERITY(),
      @ResultMessage = ERROR_MESSAGE()
  END CATCH

END