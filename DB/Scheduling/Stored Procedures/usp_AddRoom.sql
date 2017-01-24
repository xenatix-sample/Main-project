-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddRoom]
-- Author:		John Crossen
-- Date:		02/25/2016
--
-- Purpose:		Add Room details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/25/2016	John Crossen    TFS#6659		Initial creation.

-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Scheduling.usp_AddRoom
	@FacilityID INT,
	@RoomName NCHAR(255),
	@RoomCapacity INT,
	@IsSchedulable BIT,
	@IsActive BIT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT,
	@RoomID BIGINT OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

SELECT
    @ResultCode = 0,
    @ResultMessage = 'Executed Successfully'

	BEGIN TRY
	
	INSERT INTO Scheduling.Room
	        ( FacilityID ,
	          RoomName ,
	          RoomCapacity ,
			  IsSchedulable,
	          IsActive ,
	          ModifiedBy ,
	          ModifiedOn ,
	          CreatedBy ,
	          CreatedOn
	        )
	VALUES  ( @FacilityID, @RoomName, @RoomCapacity, @IsSchedulable, @IsActive, @ModifiedBy, @ModifiedOn, @ModifiedBy, @ModifiedOn)
	        ;

	SELECT @RoomID = SCOPE_IDENTITY();

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Scheduling', 'Room', @RoomID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Scheduling', 'Room', @AuditDetailID, @RoomID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;	 	 

	END TRY

	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH
END