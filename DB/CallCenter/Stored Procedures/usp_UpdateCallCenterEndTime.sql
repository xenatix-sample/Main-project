-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateCallCenterEndTime]
-- Author:		John Crossen
-- Date:		01/27/2016
--
-- Purpose:		Update Call End Time for CallCenter
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
--	Date			Author				Notes
-- 01/27/2016	John Crossen   5714	- Initial creation.
-- 06/01/2016   Gaurav Gupta   10993 - Added call End time column
-- 12/15/2016	Scott Martin	Updated auditing
-- 01/06/2017	Karl Jablonski	Removed CallStartTime from the update
--	01/11/2017		Rahul Vats			Reviewed the code and Readding the CallStartTime to be updated
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [CallCenter].[usp_UpdateCallCenterEndTime]
	@CallCenterHeaderID BIGINT,
	@CallStartTime DATETIME,
	@CallEndTime DATETIME=null,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS

BEGIN
DECLARE @AuditDetailID BIGINT,
			@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
			@ResultMessage = 'Executed Successfully'

	DECLARE @ContactID BIGINT;

	SELECT @ContactID = ContactID FROM CallCenter.CallCenterHeader WHERE CallCenterHeaderID = @CallCenterHeaderID;

	BEGIN TRY
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'CallCenter', 'CallCenterHeader', @CallCenterHeaderID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE CallCenter.CallCenterHeader
	SET CallStartTime=@CallStartTime,
		CallEndTime=@CallEndTime,
		ModifiedBy=@ModifiedBy,
		ModifiedOn=@ModifiedOn,
		SystemModifiedOn=GETUTCDATE()
	WHERE CallCenterHeaderID=@CallCenterHeaderID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'CallCenter', 'CallCenterHeader', @AuditDetailID, @CallCenterHeaderID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY
	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH
END