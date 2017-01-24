-- Procedure:	[usp_GetAppointment]
-- Author:		John Crossen
-- Date:		10/15/2015
--
-- Purpose:		Delete Appointment Header Record
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/15/2015	John Crossen    TFS#2731		Initial creation.
-- 10/31/2015   Satish Singh					Added: AppointmentID
-- 02/02/2016   Satish Singh                    Join with AppointmentContact and added ContactID, ModifiedBy and Modifiedon
-- 02/17/2016	Scott Martin	Added IsCancelled parameter to select statement
-- 03/02/2016	Scott Martin	Added CancelReasonID, CancelComment and Comments
-- 04/01/2016   Justin Spalti   Added IsInterpreterRequired and ServiceStatusID to the results set
-- 04/01/2016	Sumana Sangapu	Added NonMHMRAppointment
-- 04/11/2016	Sumana Sangapu	Added IsGroup and modified AppointmentStartTime to Time datatype
-- 04/11/2016	Sumana Sangapu	Modified the proc to use View - vw_GetAppointmentDetails
---05/02/2016   Vishal Joshi    Removed IsCancelled condition
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [Scheduling].[usp_GetAppointment]
	@AppointmentID BIGINT,
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT
AS

BEGIN
SELECT
	@ResultCode = 0,
	@ResultMessage = 'Executed Successfully'

	BEGIN TRY 
	SELECT 
		[AppointmentID],
		ResourceID as ContactID,
		[ProgramID],
		[AppointmentTypeID],
		FacilityID,
		[ServicesID],
		[ServiceStatusID],
		[AppointmentDate],
		[AppointmentStartTime],
		[AppointmentLength],
		[SupervisionVisit],
		[ReferredBy],
		[ReasonForVisit],
		[RecurrenceID],
		[IsCancelled],
		[CancelReasonID],
		[CancelComment],
		[Comments],
		[IsInterpreterRequired],
		[NonMHMRAppointment],
	--	[IsGroupAppointment],
		[ModifiedBy],
		[ModifiedOn]
	FROM
		[Scheduling].[vw_GetAppointmentDetails]
	WHERE
		AppointmentID = @AppointmentID
		AND	ResourceTypeID = 7 -- Contact
		AND IsActive=1

	END TRY

	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH

END