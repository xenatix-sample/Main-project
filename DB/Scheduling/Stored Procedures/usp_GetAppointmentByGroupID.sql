-- Procedure:	[usp_GetAppointmentByGroupID]
-- Author:		Justin Spalti
-- Date:		03/17/2016
--
-- Purpose:		Get appointment details by group id
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/17/2016 - Justin Spalti - Initial creation.
-- 04/01/2016 - Justin Spalti - Added IsInterpreterRequired and ServiceStatusID to the results set
-- 04/11/2016 -	Sumana Sangapu - Modified the proc to use View - vw_GetAppointmentDetails
-- 04/15/2016 - Justin Spalti - Added RecurrenceID to the results set
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [Scheduling].[usp_GetAppointmentByGroupID]
	@GroupHeaderID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS

BEGIN
SELECT
	@ResultCode = 0,
	@ResultMessage = 'Executed Successfully';

	BEGIN TRY 

		SELECT TOP 1	
			A.[AppointmentID],
			A.[ProgramID],
			A.[AppointmentTypeID],
			A.FacilityID,
			A.[ServicesID],
			A.[ServiceStatusID],
			A.[AppointmentDate],
			A.[AppointmentStartTime],
			A.[AppointmentLength],
			A.[SupervisionVisit],
			A.[ReferredBy],
			A.[ReasonForVisit],
			A.[IsCancelled],
			A.[CancelReasonID],
			A.[CancelComment],
			A.[Comments],
			A.[IsInterpreterRequired],
			A.[RecurrenceID],
			A.[ModifiedBy],
			A.[ModifiedOn]
		FROM	[Scheduling].[vw_GetAppointmentDetails] a
		WHERE	a.GroupHeaderID = @GroupHeaderID
			AND	a.IsActive = 1
			AND	a.IsCancelled = 0;

	END TRY
	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE();
	END CATCH;
END;