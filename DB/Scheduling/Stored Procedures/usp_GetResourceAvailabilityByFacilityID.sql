-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetResourceAvailabilityByFacilityID]
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

Create PROCEDURE [Scheduling].[usp_GetResourceAvailabilityByFacilityID]
	@ResourceID INT = NULL,
	@ResourceTypeID SMALLINT = NULL,
	@FacilityID SMALLINT,
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
		AND RA.FacilityID = @FacilityID
		AND RA.IsActive=1

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END