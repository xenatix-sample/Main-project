
-- Procedure:	[usp_UpdateAppointmentNoShow]
-- Author:		John Crossen
-- Date:		3/09/2016
--
-- Purpose:		Update No Show
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 3/9/2016	 John Crossen    TFS#7583		Initial creation.

-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_UpdateAppointmentNoShow]
	@AppointmentResourceID BIGINT,
	@IsNoShow BIT,
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
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Scheduling', 'AppointmentResource', @AppointmentResourceID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE [Scheduling].[AppointmentResource]    
	SET @IsNoShow=@IsNoShow,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		AppointmentResourceID = @AppointmentResourceID

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Scheduling', 'AppointmentResource', @AuditDetailID, @AppointmentResourceID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH

END