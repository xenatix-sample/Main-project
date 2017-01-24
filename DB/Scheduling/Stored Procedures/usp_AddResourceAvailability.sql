-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddResourceAvailability]
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
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added CreatedBy and CreatedOn field
-- 02/15/2016	Scott Martin		Changed Days to DayOfWeek and type from nvarchar(10) to smallint
-- 02/29/2016	Scott Martin		Added ScheduleType parameter
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Scheduling.usp_AddResourceAvailability
	@ResourceID INT,
	@ResourceTypeID SMALLINT,
	@FacilityID INT,
	@DefaultFacilityID INT,
	@DayOfWeekID INT,
	@AvailabilityStartTime NVARCHAR(10),
	@AvailabilityEndTime NVARCHAR(10),
	@ScheduleTypeID SMALLINT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT,
	@ResourceAvailabilityID BIGINT OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

SELECT
    @ResultCode = 0,
    @ResultMessage = 'Executed Successfully'

	BEGIN TRY
	INSERT INTO Scheduling.ResourceAvailability
	(
		ResourceID,
		ResourceTypeID,
		FacilityID,
		DefaultFacilityID,
		ScheduleTypeID,
		[DayOfWeekID],
		AvailabilityStartTime,
		AvailabilityEndTime,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	VALUES
	(
		@ResourceID,
		@ResourceTypeID,
		@FacilityID,
		@DefaultFacilityID,
		@ScheduleTypeID,
		@DayOfWeekID,
		@AvailabilityStartTime,
		@AvailabilityEndTime,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);

	SELECT @ResourceAvailabilityID = SCOPE_IDENTITY();

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Scheduling', 'ResourceAvailability', @ResourceAvailabilityID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Scheduling', 'ResourceAvailability', @AuditDetailID, @ResourceAvailabilityID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;	 	 

	END TRY

	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH
END