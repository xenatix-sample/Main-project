-- Procedure:	[usp_DeleteAppointmentContact]
-- Author:		John Crossen
-- Date:		10/15/2015
--
-- Purpose:		Delete AppointmentContact Record
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/15/2015	John Crossen    TFS#2590		Initial creation.
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added SystemModifiedOn field
-- 02/17/2016	Scott Martin		Added audit logging
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_DeleteAppointmentContact]
	@AppointmentContactID BIGINT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS

BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

SELECT
    @ResultCode = 0,
    @ResultMessage = 'Executed Successfully';

	BEGIN TRY
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Scheduling', 'AppointmentContact', @AppointmentContactID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	
	UPDATE [Scheduling].[AppointmentContact]
	SET IsActive = 0,
		[ModifiedBy] = @ModifiedBy,
		[ModifiedOn] = @ModifiedOn,
		SystemModifiedOn=GETUTCDATE()
	WHERE
		AppointmentContactID = @AppointmentContactID;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Scheduling', 'AppointmentContact', @AuditDetailID, @AppointmentContactID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH

END
GO


