-- Procedure:	[usp_DeleteRecurrence]
-- Author:		John Crossen
-- Date:		3/16/2016
--
-- Purpose:		deleted recurrence for an appointment
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 3/16/2016	Chad Roberts    		Initial creation.
-----------------------------------------------------------------------------------------------------------------------

create PROCEDURE [Scheduling].usp_DeleteRecurrence
	@RecurrenceID BIGINT,
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
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Scheduling', 'Recurrence', @RecurrenceID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;			

	UPDATE [Scheduling].[Recurrence]
	SET IsActive = 0,
		[ModifiedBy] = @ModifiedBy,
		[ModifiedOn] = @ModifiedOn
	WHERE
		RecurrenceID = @RecurrenceID;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Scheduling', 'Recurrence', @AuditDetailID, @RecurrenceID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH
END