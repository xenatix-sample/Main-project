-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetScheduleTypes]
-- Author:		Scott Martin
-- Date:		02/14/2016
--
-- Purpose:		Gets the list of Appointment Availability by Facility
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/14/2016	Scott Martin		Initial creation.
-----------------------------------------------------------------------------------------------------------------------

Create PROCEDURE [Scheduling].[usp_GetScheduleTypes]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	

	SELECT
		ScheduleTypeID,
		ScheduleType,
		IsSystem 
	FROM
		Scheduling.ScheduleType
	WHERE
		IsActive = 1

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END