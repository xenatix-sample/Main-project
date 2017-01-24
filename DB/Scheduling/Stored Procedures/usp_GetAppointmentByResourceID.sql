-- Procedure:	[usp_GetAppointmentByResource]
-- Author:		Justin Spalti
-- Date:		03/17/2016
--
-- Purpose:		Get all appoinments scheduled for resource
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/17/2016 - Justin Spalti - Initial Creation
-- 04/13/2016 - Sumana Sangapu	- Added the view
-- 04/29/2016 - Justin Spalti - Added FacilityID to the results set
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [Scheduling].[usp_GetAppointmentByResourceID]
	@ResourceID INT,
	@ResourceTypeID SMALLINT,
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT
AS

BEGIN
SELECT
    @ResultCode = 0,
    @ResultMessage = 'Executed Successfully'

	BEGIN TRY

				SELECT			AppointmentResourceID,
								AppointmentID,
								ResourceID,
								ResourceTypeID,
								ParentID,
								ProgramID,
								AppointmentTypeID,
								ServicesID,
								AppointmentDate,
								AppointmentStartTime,
								AppointmentLength,
								SupervisionVisit,
								ReferredBy,
								ReasonForVisit,
								RecurrenceID,
								IsCancelled,
								CancelReasonID,
								CancelComment,
								Comments,
								IsBlocked,
								IsGroupAppointment,
								GroupDetailID,
								GroupHeaderID as GroupID,
								AppointmentStatusID,
								GroupType,
								FacilityID
					FROM	Scheduling.vw_GetAppointmentDetails 
					WHERE	ResourceID = @ResourceID
					AND		ResourceTypeID = @ResourceTypeID -- Contact - ResourceType
					AND		IsActive = 1
				--	AND		IsCancelled = 0 -- NotCancelled 


	END TRY

	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH

END