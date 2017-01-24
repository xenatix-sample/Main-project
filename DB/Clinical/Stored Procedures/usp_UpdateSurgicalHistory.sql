-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_UpdateSurgicalHistory]
-- Author:		Scott Martin
-- Date:		11/30/2015
--
-- Purpose:		Update Surgical History
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/30/2015	Scott Martin	Initial creation.
-- 11/30/2015	Scott Martin	Changed column TakenOn to TakenTime
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added SystemModifiedOn to Update statement
-- 02/17/2016	Scott Martin	Refactored audit logging
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_UpdateSurgicalHistory]
	@SurgicalHistoryID BIGINT,
	@ContactID BIGINT,
	@EncounterID BIGINT NULL,
	@TakenBy INT,
	@TakenTime DATETIME,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Clinical', 'SurgicalHistory', @SurgicalHistoryID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Clinical.SurgicalHistory
	SET ContactID = @ContactID,
		EncounterID = @EncounterID,
		TakenBy = @TakenBy,
		TakenTime = @TakenTime,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		SurgicalHistoryID = @SurgicalHistoryID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Clinical', 'SurgicalHistory', @AuditDetailID, @SurgicalHistoryID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

 	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


