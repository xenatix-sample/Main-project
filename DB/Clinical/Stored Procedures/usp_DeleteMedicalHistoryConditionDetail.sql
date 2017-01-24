-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_DeleteMedicalHistoryConditionDetail]
-- Author:		Scott Martin
-- Date:		11/20/2015
--
-- Purpose:		Delete Medical history condition detail
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/2/2015	Scott Martin	Initial creation.
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added CreatedBy and CreatedOn to Insert
-- 02/17/2016	Scott Martin	Added audit logging
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_DeleteMedicalHistoryConditionDetail]
	@MedicalHistoryConditionDetailID BIGINT,
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

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Clinical', 'MedicalHistoryConditionDetail', @MedicalHistoryConditionDetailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Clinical.MedicalHistoryConditionDetail
	SET IsActive = 0,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		MedicalHistoryConditionDetailID = @MedicalHistoryConditionDetailID;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Clinical', 'MedicalHistoryConditionDetail', @AuditDetailID, @MedicalHistoryConditionDetailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

 	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

GO