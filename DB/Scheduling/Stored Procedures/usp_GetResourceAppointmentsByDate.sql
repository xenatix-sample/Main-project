-----------------------------------------------------------------------------------------------------------------------

-- Procedure:	[usp_GetResourceAppointmentsByDate]
-- Author:		Scott Martin
-- Date:		11/25/2015
--
-- Purpose:		Get all appointments for a date range and specific resource
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/25/2015	Scot Martin    TFS#3411		Initial creation.
-- 02/17/2016	Scott Martin	Added IsCancelled parameter to select statement
-- 04/11/2016	Sumana Sangapu	Modified the proc to use View - vw_GetAppointmentDetails
-- 04/13/2016	Sumana Sangapu	Return ServiceName and Group details
-- 04/14/2016	Sumana Sangapu	Removed the filter on IsCancelled
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_GetResourceAppointmentsByDate]
	@ResourceID INT,
	@ResourceTypeID INT,
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
 
					SELECT				AppointmentResourceID,
										AppointmentID,
										[ProgramID],
										[AppointmentTypeID],
										[ServicesID],
										[AppointmentDate],
										[AppointmentStartTime],
										[AppointmentLength],
										[SupervisionVisit],
										[ReferredBy],
										[ReasonForVisit],
										RecurrenceID,
										[IsCancelled],
										[CancelReasonID],
										[CancelComment],
										[Comments],
										IsBlocked,
										IsGroupAppointment,
										GroupDetailID,
										GroupHeaderID as GroupID,
										AppointmentStatusID,
										GroupName,
										AppointmentType,
										ServiceName ,
										GroupType,
										ProgramName,
										FacilityName,
										Comments,
										GroupComments

							FROM	Scheduling.vw_GetAppointmentDetails 
							WHERE	[ResourceID] = @ResourceID 
							AND		[ResourceTypeID] = @ResourceTypeID
							AND		AppointmentDate BETWEEN @StartDate AND @EndDate
							AND		IsActive = 1
							AND		IsCancelled = 0 -- NotCancelled 
							
	
  END TRY
  BEGIN CATCH
    SELECT
      @ResultCode = ERROR_SEVERITY(),
      @ResultMessage = ERROR_MESSAGE()
  END CATCH

END