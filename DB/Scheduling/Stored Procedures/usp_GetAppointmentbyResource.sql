
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_GetAppointmentByResource
-- Author:		Rajiv Ranjan
-- Date:		10/23/2015
--
-- Purpose:		Get all appoinments scheduled for resource
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/23/2015	Rajiv Ranajn    Initial creation.
-- 02/17/2016	Scott Martin	Added IsCancelled parameter to select statement
-- 03/02/2016	Scott Martin	Added CancelReasonID, CancelComment, and Comments
-- 03/29/2016	Sumana Sangapu	Refactored proc to replace AppointmentContact
-- 04/01/2016	Sumana Sangapu	Code to return group appointments to the grid
-- 04/11/2016	Sumana Sangapu	Return all the appointments by Resource - fine tuned and included GroupDetails , Status in result set
-- 04/11/2016	Sumana Sangapu	Modified the proc to use View - vw_GetAppointmentDetails
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Scheduling.usp_GetAppointmentByResource
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

					SELECT		AppointmentResourceID,
								AppointmentID,
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
								--IsGroupAppointment,
								GroupDetailID,
								GroupHeaderID,
								AppointmentStatusID
					FROM	Scheduling.vw_GetAppointmentDetails 
					WHERE	ResourceID = @ResourceID
					AND		ResourceTypeID = @ResourceTypeID -- Contact - ResourceType
					AND		IsActive = 1
					AND		IsCancelled = 0 -- NotCancelled 


	END TRY

	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH

END