-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetResourceAvailability]
-- Author:		John Crossen
-- Date:		10/16/2015
--
-- Purpose:		Gets the list of Appointment Availability
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/16/2015	John Crossen	TFS# 2765 - Initial creation.
-- 01/15/2016    Satish Singh    @ResourceTypeID, @ResourceID  optional for where clause
-- 02/14/2016	Scott Martin		Added ScheduleTypeID
-----------------------------------------------------------------------------------------------------------------------

Create PROCEDURE [Scheduling].[usp_GetResourceAvailability]
	@ResourceID INT = NULL,
	@ResourceTypeID SMALLINT = NULL,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	

	SELECT
		RA.ResourceAvailabilityID,
		RA.ResourceID,
		RA.ResourceTypeID,
		RA.FacilityID,
		RA.DefaultFacilityID,
		RA.ScheduleTypeID,
		RA.[DayOfWeekID],
		DW.Name AS [DAYS],
		RA.AvailabilityStartTime,
		RA.AvailabilityEndTime 
	FROM
		Scheduling.ResourceAvailability RA
		INNER JOIN Reference.DayOfWeek DW
			ON RA.DayOfWeekID = DW.DayOfWeekID
	WHERE
		(ISNULL(@ResourceID,0) = 0 OR ResourceID = @ResourceID) 
		AND (ISNULL(@ResourceTypeID, 0) = 0 OR ResourceTypeID = @ResourceTypeID)
		AND RA.IsActive = 1

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END