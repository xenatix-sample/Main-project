-- Procedure:	[usp_DeleteAppointmentReoccuring]
-- Author:		John Crossen
-- Date:		2/16/2016
--
-- Purpose:		Delete Appointment Reoccuring Record
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 2/16/2016	 John Crossen    TFS#5971		Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_DeleteAppointmentReoccuring]
	@AppointmentReoccuringID BIGINT,
	@ModifiedBy INT,
	@ModifiedOn DATETIME,
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
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Scheduling', 'AppointmentReoccuring', @AppointmentReoccuringID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE [Scheduling].[AppointmentReoccuring]        
	SET IsActive = 0,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		[AppointmentReoccuringID] = @AppointmentReoccuringID;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Scheduling', 'AppointmentReoccuring', @AuditDetailID, @AppointmentReoccuringID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH

END