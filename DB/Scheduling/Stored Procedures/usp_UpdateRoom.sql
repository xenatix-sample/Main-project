-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateRoom]
-- Author:		John Crossen
-- Date:		2/25/2016
--
-- Purpose:		Update Room details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/25/2016	John Crossen    TFS#6659		Initial creation.

-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Scheduling.usp_UpdateRoom
	@RoomID INT,
	@FacilityID INT,
	@RoomName NCHAR(255),
	@RoomCapacity INT,
	@IsSchedulable BIT,
	@IsActive BIT,
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
    @ResultMessage = 'Executed Successfully'

	BEGIN TRY

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Scheduling', 'Room', @RoomID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;			


	
	UPDATE Scheduling.Room SET 
	FacilityID=@FacilityID, 
	RoomName=@RoomName, 
	RoomCapacity=@RoomCapacity, 
	IsSchedulable=@IsSchedulable,
	IsActive=@IsActive, 
	ModifiedBy=@ModifiedBy, 
	ModifiedOn=@ModifiedOn,
	SystemModifiedOn = GETUTCDATE()
	WHERE RoomID=@RoomID


	
	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Scheduling', 'Room', @AuditDetailID, @RoomID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH
END