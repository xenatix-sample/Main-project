-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetSchedulingResourceByAppointment]
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
-- 02/03/2016   Satish Singh                Added IsAvtive used in offline data
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_GetSchedulingResourceByAppointment]
	@AppointmentID INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY

		SELECT 
		    R.AppointmentResourceID,
			R.AppointmentID,
			R.ResourceID,
			R.ResourceTypeID,
			R.ParentID,
			R.IsActive,
			ApptStatus.AppointmentStatusDetailID,
			ApptStatus.AppointmentStatusID
			
		FROM 
			Scheduling.AppointmentResource R
			OUTER APPLY (SELECT TOP 1 S.AppointmentStatusID, S.AppointmentStatusDetailID 
								 FROM Scheduling.AppointmentStatusDetails S
								 WHERE S.AppointmentResourceID = R.AppointmentResourceID 
								 AND S.IsActive = 1 ORDER BY S.ModifiedOn DESC) AS ApptStatus
		WHERE 
			R.AppointmentID = @AppointmentID AND R.IsActive = 1

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END