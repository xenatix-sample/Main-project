-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_DeleteResourceAvailability]
-- Author:		John Crossen
-- Date:		10/05/2015
--
-- Purpose:		Delete Resource Availability details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/05/2015	John Crossen    TFS#2590		Initial creation.
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added SystemModifiedOn field
-- 02/15/2016	Scott Martin		Changed Days to DayOfWeek and type from nvarchar(10) to smallint
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE Scheduling.usp_DeleteResourceAvailability
	@ResourceAvailabilityID BIGINT,
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
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Scheduling', 'ResourceAvailability', @ResourceAvailabilityID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
   
	UPDATE Scheduling.ResourceAvailability
	SET IsActive = 0,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ResourceAvailabilityID = @ResourceAvailabilityID;
  
  	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Scheduling', 'ResourceAvailability', @AuditDetailID, @ResourceAvailabilityID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	
	END TRY

	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH
END