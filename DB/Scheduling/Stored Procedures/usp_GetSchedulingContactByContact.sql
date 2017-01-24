
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetSchedulingContactByContact]
-- Author:		John Crossen
-- Date:		10/15/2015
--
-- Purpose:		Get Scheduling Resource Details
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/15/2015	John Crossen	TFS#2731	Initial draft.
-- 10/16/2015   John Crossen    TFS# 2765   Add Apointment information
-- 02/02/2016   Satish Singh                Added ContactID and FacilityID
-- 02/03/2016   Satish Singh                Condition for active appointment
-- 02/12/2016   Satish Singh                Condition for cancel appointment
-- 03/02/2016	Scott Martin	Added CancelReasonID, CancelComment, and Comments
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_GetSchedulingContactByContact]
	@ContactID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY

		SELECT
		    A.AppointmentID,
			R.ContactID,
			A.ProgramID,
			A.AppointmentTypeID,
			A.FacilityID,
			A.ServicesID,
			A.AppointmentDate,
			A.AppointmentStartTime,
			A.AppointmentLength,
			A.SupervisionVisit,
			A.ReferredBy,
			A.ReasonForVisit,
			A.RecurrenceID,
			A.IsCancelled,
			A.CancelReasonID,
			A.CancelComment,
			A.Comments
			
		FROM 
			Scheduling.AppointmentContact R
			JOIN Scheduling.Appointment A ON R.AppointmentID=A.AppointmentID

		WHERE 
			R.ContactID = @ContactID AND R.IsActive = 1 AND A.IsActive=1 AND A.IsCancelled=0

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END