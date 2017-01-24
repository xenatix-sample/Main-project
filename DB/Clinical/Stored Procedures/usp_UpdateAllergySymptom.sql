-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_UpdateAllergySymptom]
-- Author:		John Crossen
-- Date:		10/27/2015
--
-- Purpose:		Update AllergySymptom
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/27/2015	John Crossen	TFS# 2892 - Initial creation.
-- 11/18/2015	Scott Martin	TFS 3610	Add audit logging
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added SystemModifiedOn to Update statement
-- 02/17/2016	Scott Martin	Refactored audit logging
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_UpdateAllergySymptom]
	@AllergySymptomID BIGINT,
	@AllergySymptom NVARCHAR(255),
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

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Clinical', 'AllergySymptom', @AllergySymptomID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Clinical.AllergySymptom
	SET AllergySymptom = @AllergySymptom,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		AllergySymptomID = @AllergySymptomID

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Clinical', 'AllergySymptom', @AuditDetailID, @AllergySymptomID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
  
 	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO