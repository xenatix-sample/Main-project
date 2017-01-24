-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetSchedulingResourceByResource]
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
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_GetSchedulingResourceByResource]
	@ResourceID INT,
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
		WHERE 
			R.ResourceID = @ResourceID AND R.IsActive = 1

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END