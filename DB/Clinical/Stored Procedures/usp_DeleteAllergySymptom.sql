-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_DeleteAllergySymptom]
-- Author:		John Crossen
-- Date:		10/27/2015
--
-- Purpose:		Delete AllergySymptom
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/27/2015	John Crossen	TFS# 2892 - Initial creation.
-- 11/17/2015	Scott Martin	TFS 3610	Add audit logging
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added CreatedBy and CreatedOn to Insert
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_DeleteAllergySymptom]
	@AllergySymptomID BIGINT,
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

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Clinical', 'AllergySymptom', @AllergySymptomID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Clinical.AllergySymptom
	SET IsActive = 0,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		AllergySymptomID = @AllergySymptomID;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Clinical', 'AllergySymptom', @AuditDetailID, @AllergySymptomID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

 	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO