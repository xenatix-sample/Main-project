-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetSchedulingResourceByContact]
-- Author:		Rajiv Ranjan
-- Date:		10/23/2015
--
-- Purpose:		Get Scheduling Resource Details
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/23/2015	Rajiv Ranjan	Initial draft.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_GetSchedulingResourceByContact]
	@ContactID INT,
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
			R.ParentID
			
		FROM 
			Scheduling.AppointmentResource R
			INNER JOIN Scheduling.Appointment A ON R.AppointmentID = A.AppointmentID
			INNER JOIN Scheduling.AppointmentContact AC ON A.AppointmentID = AC.AppointmentID
		WHERE 
			AC.ContactID = @ContactID 
			AND R.IsActive = 1
			AND A.IsActive = 1
			AND AC.IsActive = 1

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END