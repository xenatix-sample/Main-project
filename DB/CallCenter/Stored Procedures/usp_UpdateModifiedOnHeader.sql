-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateModifiedOnHeader]
-- Author:		Deepak Kumar
-- Date:		08/17/2016
--
-- Purpose:		Update for Header Modified On
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/17/2016	Deepak Kumar	Initial creation.
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [CallCenter].[usp_UpdateModifiedOnHeader]
	@CallCenterHeaderID BIGINT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
    @ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
    BEGIN TRY
	DECLARE @AuditDetailID BIGINT,
			@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);
	SELECT	@ResultCode = 0,
			@ResultMessage = 'executed successfully'

			DECLARE @ContactID BIGINT;

			SELECT @ContactID = ContactID FROM CallCenter.CallCenterHeader WHERE CallCenterHeaderID = @CallCenterHeaderID;

			EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'CallCenter', 'CallCenterHeader', @CallCenterHeaderID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			UPDATE CallCenter.CallCenterHeader 
			SET ModifiedBy = @ModifiedBy, 
				ModifiedOn = @ModifiedOn, 
				SystemModifiedOn = GETUTCDATE()
			WHERE CallCenterHeaderID = @CallCenterHeaderID

			EXEC Auditing.usp_AddPostAuditLog 'Update', 'CallCenter', 'CallCenterHeader', @AuditDetailID, @CallCenterHeaderID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY
	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH
END