-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateResourceAvailability]
-- Author:		John Crossen
-- Date:		10/05/2015
--
-- Purpose:		Add Resource Availability details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/05/2015	John Crossen    TFS#2590		Initial creation.
-- 02/15/2016	Scott Martin		Changed Days to DayOfWeek and type from nvarchar(10) to smallint
-- 02/29/2016	Scott Martin		Added ScheduleType paramter
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Scheduling.usp_UpdateResourceAvailability
	@ResourceAvailabilityID BIGINT,
	@ResourceID INT,
	@ResourceTypeID SMALLINT,
	@FacilityID INT,
	@DefaultFacilityID INT,
	@ScheduleTypeID SMALLINT,
	@DayOfWeekID INT,
	@AvailabilityStartTime NVARCHAR(10),
	@AvailabilityEndTime NVARCHAR(10),
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT	
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

SELECT
    @ResultCode = 0,
    @ResultMessage = 'Executed Successfully';

	BEGIN TRY
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Scheduling', 'ResourceAvailability', @ResourceAvailabilityID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Scheduling.ResourceAvailability
	SET ResourceID = @ResourceID,
        ResourceTypeID = @ResourceTypeID,
        FacilityID = @FacilityID,
        DefaultFacilityID = @DefaultFacilityID,
		ScheduleTypeID = @ScheduleTypeID,
        [DayOfWeekID] = @DayOfWeekID,
        AvailabilityStartTime = @AvailabilityStartTime ,
        AvailabilityEndTime = @AvailabilityEndTime ,
        ModifiedBy = @ModifiedBy ,
        ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ResourceAvailabilityID = @ResourceAvailabilityID;
  
	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Scheduling', 'ResourceAvailability', @AuditDetailID, @ResourceAvailabilityID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH
END